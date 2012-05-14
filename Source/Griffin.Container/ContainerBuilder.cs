using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Builds the container.
    /// </summary>
    public class ContainerBuilder : IContainerBuilder
    {
        private readonly Dictionary<Type, ConcreteBuildPlan> _buildPlans = new Dictionary<Type, ConcreteBuildPlan>();
        private readonly Dictionary<Type, List<IBuildPlan>> _serviceMappings = new Dictionary<Type, List<IBuildPlan>>();
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
                    List<IBuildPlan> buildPlans;
                    if (!_serviceMappings.TryGetValue(service, out buildPlans))
                    {
                        buildPlans = new List<IBuildPlan>();
                        _serviceMappings.Add(service, buildPlans);
                    }

                    buildPlans.Add(_buildPlans[registration.ConcreteType]);
                }
            }
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
                List<IBuildPlan> bp;
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
                var buildPlan = new ConcreteBuildPlan(registration.ConcreteType, registration.Lifetime, strategy);
                    ConstructorInfo constructor;
                    var error = TryGetConstructor(registration.ConcreteType, out constructor);
                    if (error != null)
                        throw new TypeResolutionFailedException(error);

                    buildPlan.SetConstructor(constructor);
                    _buildPlans.Add(registration.ConcreteType, buildPlan);
                }
                else
                {
                    foreach (var service in registration.Services)
                    {
                        List<IBuildPlan> buildPlans;
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
        protected virtual TypeResolutionFailed TryGetConstructor(Type concreteType, out ConstructorInfo constructor)
        {
            var error = new TypeResolutionFailed(concreteType);
            foreach (var constructorInfo in concreteType.GetConstructors().OrderBy(x => x.GetParameters().Length))
            {
                var found =
                    constructorInfo.GetParameters().All(
                        parameter => _registrar.Registrations.Any(x => x.Implements(parameter.ParameterType)));

                if (found)
                {
                    constructor = constructorInfo;
                    return null;
                }


                error.Add(new ConstructorFailedReason(constructorInfo, "Failed to resolve "));
            }

            constructor = null;
            return error;
        }

        //public class FindConstructor
    }
}