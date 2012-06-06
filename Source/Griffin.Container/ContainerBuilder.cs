using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Griffin.Container.BuildPlans;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Builds the container.
    /// </summary>
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly Dictionary<Type, ConcreteBuildPlan> _buildPlans = new Dictionary<Type, ConcreteBuildPlan>();
        private readonly IServiceMappings _serviceMappings = new ServiceMappings();
        private IContainerRegistrar _registrar;

        #region IContainerBuilder Members

        /// <summary>
        /// Builds a container using the specified registrations.
        /// </summary>
        /// <param name="registrar">Registrations to use</param>
        /// <returns>A created container.</returns>
        /// <remarks>Will analyze all registrations and create a build plan for each service</remarks>
        public IParentContainer Build(IContainerRegistrar registrar)
        {
            if (registrar == null) throw new ArgumentNullException("registrar");

            _registrar = registrar;
            GenerateBuildPlans(registrar);
            MapConcretesToServices();
            BindPlans();
            Validate();
            return new Container(_serviceMappings);
        }

        #endregion

        private void MapConcretesToServices()
        {
            foreach (var registration in _registrar.Registrations)
            {
                if (registration.ConcreteType == null)
                    continue;

                foreach (var service in registration.Services)
                {
                    IList<IBuildPlan> buildPlans;
                    if (!_serviceMappings.TryGetValue(service, out buildPlans))
                    {
                        buildPlans = new List<IBuildPlan>();
                        _serviceMappings.Add(service, buildPlans);
                    }

                    /*
                    if (buildPlans.Any() && registration.Lifetime != buildPlans.First().Lifetime)
                        throw new InvalidOperationException(
                            string.Format(
                                "Concretes which implements the same service may not have different lifetimes. Which {0} and {1} has.",
                                registration.ConcreteType.FullName, buildPlans.First().DisplayName));
                    */

                    buildPlans.Add(_buildPlans[registration.ConcreteType]);
                }
            }
        }

        /// <summary>
        /// Validate that all services can be built.
        /// </summary>
        protected virtual void Validate()
        {
            foreach (var mapping in _serviceMappings)
            {
                var breadcrumbs = new LinkedList<Type>();
                ValidateMapping(mapping.Key, breadcrumbs);
            }
        }

        private void ValidateMapping(Type service, LinkedList<Type> breadcrumbs)
        {
            if (breadcrumbs.Contains(service))
                throw new CircularDependenciesException(
                    "Found a circular dependency when looking up " + breadcrumbs.First() +
                    ", when inspecting the constructor of " + breadcrumbs.Last() + ", violating service: " + service,
                    breadcrumbs);



            IList<IBuildPlan> buildPlans;
            if (!_serviceMappings.TryGetValue(service, out buildPlans))
                throw new DependencyNotRegisteredException(breadcrumbs.Last.Value, service);

            var cbp = buildPlans.Last() as ConcreteBuildPlan;
            if(cbp == null)
                return;

            breadcrumbs.AddLast(service);
            foreach (var parameter in cbp.Constructor.GetParameters())
            {
                ValidateMapping(parameter.ParameterType, breadcrumbs);
            }
            breadcrumbs.RemoveLast();
        }

        /// <summary>
        /// Go through each plan add add the constructor parameter plans to it.
        /// </summary>
        private void BindPlans()
        {
            foreach (var buildPlan in _buildPlans.Values)
            {
                BindBuildPlan(buildPlan);
            }
        }

        private void BindBuildPlan(ConcreteBuildPlan buildPlan)
        {
            var parameters = buildPlan.Constructor.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                IList<IBuildPlan> bp;
                if (!_serviceMappings.TryGetValue(parameters[i].ParameterType, out bp))
                    throw new InvalidOperationException(string.Format("Failed to find service {0}.",
                                                                      parameters[i].ParameterType));
                buildPlan.AddConstructorPlan(i, bp[0]);
            }
        }

        /// <summary>
        /// Used to create the correct instance strategy
        /// </summary>
        /// <param name="registration">Registration information</param>
        /// <returns>Strategy to use.</returns>
        protected virtual IInstanceStrategy CreateStrategy(ComponentRegistration registration)
        {
            switch (registration.Lifetime)
            {
                case Lifetime.Transient:
                    return new TransientInstanceStrategy();
                case Lifetime.Scoped:
                    return new ScopedInstanceStrategy(registration.ConcreteType);
                case Lifetime.Singleton:
                    return new SingletonFactoryStrategy();
                default:
                    throw new NotSupportedException("Unsupported lifetime: " + registration.Lifetime);
            }
        }

        /// <summary>
        /// Go through all registrations and lookup their dependencies.
        /// </summary>
        /// <param name="registrar"></param>
        private void GenerateBuildPlans(IContainerRegistrar registrar)
        {
            foreach (var registration in registrar.Registrations)
            {
                var strategy = registration.InstanceStrategy ?? CreateStrategy(registration);

                if (!strategy.IsInstanceFactory)
                {
                    var buildPlan = registration.ConcreteType.IsGenericType
                        ? new GenericBuildPlan(registration.ConcreteType, registration.Lifetime, strategy)
                        : new ConcreteBuildPlan(registration.ConcreteType, registration.Lifetime, strategy);

                    ConstructorInfo constructor;
                    var error = TryGetConstructor(registration.ConcreteType, out constructor);
                    if (error != null)
                        throw new TypeResolutionFailedException(registration.ConcreteType, error);

                    buildPlan.SetConstructor(constructor);
                    _buildPlans.Add(registration.ConcreteType, buildPlan);
                }
                else
                {
                    foreach (var service in registration.Services)
                    {
                        IList<IBuildPlan> buildPlans;
                        if (!_serviceMappings.TryGetValue(service, out buildPlans))
                        {
                            buildPlans = new List<IBuildPlan>();
                            _serviceMappings.Add(service, buildPlans);
                        }

                        var bp = new ExternalBuildPlan(registration.Lifetime, strategy);
                        buildPlans.Add(bp);
                    }

                }
            }
        }

        /// <summary>
        /// Try to find a constructor by looking at the most specific first.
        /// </summary>
        /// <param name="concreteType">Type to create</param>
        /// <param name="constructor">Chosen constructor</param>
        /// <returns>Error if any; otherwise null.</returns>
        protected virtual FailureReasons TryGetConstructor(Type concreteType, out ConstructorInfo constructor)
        {
            var error = new FailureReasons(concreteType);
            foreach (var constructorInfo in concreteType.GetConstructors().OrderByDescending(x => x.GetParameters().Length))
            {
                var missing =
                    constructorInfo.GetParameters().FirstOrDefault(
                        parameter => !_registrar.Registrations.Any(x => x.Implements(parameter.ParameterType)));

                if (missing == null)
                {
                    constructor = constructorInfo;
                    return null;
                }


                error.Add(new ConstructorFailedReason(constructorInfo, missing.ParameterType));
            }

            constructor = null;
            return error;
        }
    }
}