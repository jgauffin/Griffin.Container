using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// Builds the container.
    /// </summary>
    public class ContainerBuilder : IContainerBuilder
    {
        readonly Dictionary<Type, BuildPlan> _buildPlans = new Dictionary<Type, BuildPlan>();
        readonly Dictionary<Type, List<BuildPlan>> _serviceMappings = new Dictionary<Type, List<BuildPlan>>();
        IContainerRegistrar _registrar;


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
            BindPlans();
            MapServices();

            return new Container(_serviceMappings);
        }

        private void MapServices()
        {
            foreach (var registration in _registrar.Registrations)
            {
                foreach (var service in registration.Services)
                {
                    List<BuildPlan> buildPlans;
                    if (!_serviceMappings.TryGetValue(service, out buildPlans))
                    {
                        buildPlans = new List<BuildPlan>();
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
                var parameters = buildPlan.Constructor.GetParameters();
                for (int i = 0; i < parameters.Length; i++)
                {
                    var bp = _buildPlans[parameters[i].ParameterType];
                    buildPlan.AddConstructorPlan(i, bp);
                }
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
                var buildPlan = new BuildPlan(registration.ConcreteType, registration.Lifetime);

                ConstructorInfo constructor;
                var error = TryGetConstructor(registration.ConcreteType, out constructor);
                if (error != null)
                    throw new TypeResolutionFailedException(error);

                buildPlan.SetConstructor(constructor);

                _buildPlans.Add(registration.ConcreteType, buildPlan);
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