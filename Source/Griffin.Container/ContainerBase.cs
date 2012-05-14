using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Base class for the container.
    /// </summary>
    public abstract class ContainerBase
    {
        private readonly IDictionary<Type, List<IBuildPlan>> _serviceMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBase"/> class.
        /// </summary>
        /// <param name="serviceMappings">The service mappings.</param>
        protected ContainerBase(IDictionary<Type, List<IBuildPlan>> serviceMappings)
        {
            if (serviceMappings == null) throw new ArgumentNullException("serviceMappings");
            _serviceMappings = serviceMappings;
        }

        /// <summary>
        /// Get all service mappings
        /// </summary>
        protected IDictionary<Type, List<IBuildPlan>> ServiceMappings
        {
            get { return _serviceMappings; }
        }

        /// <summary>
        /// Checks if a component have been registered.
        /// </summary>
        /// <param name="type">Service which is requested.</param>
        /// <returns>true if registered; otherwise false.</returns>
        public bool IsRegistered(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return ServiceMappings.ContainsKey(type);
        }

        /// <summary>
        /// Resolve a service.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>object which implements the service.</returns>
        public T Resolve<T>() where T : class
        {
            return (T) Resolve(typeof (T));
        }

        /// <summary>
        /// Resolve a service
        /// </summary>
        /// <param name="service">Requested service</param>
        /// <returns>object which implements the service</returns>
        public object Resolve(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            var bps = GetBuildPlans(service);

            var instance = GetInstance(bps[0]);
            if (instance == null)
                throw new InvalidOperationException(string.Format("Failed to construct {0}", service.FullName));

            return instance;
        }

        /// <summary>
        /// Gets the build plans for all concretes that implements a service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>Build plans</returns>
        protected virtual List<IBuildPlan> GetBuildPlans(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            List<IBuildPlan> bps;
            if (!ServiceMappings.TryGetValue(service, out bps))
                throw new InvalidOperationException(string.Format("Service {0} has not been registered.",
                                                                  service.FullName));

            return bps;
        }

        /// <summary>
        /// Get instance for the specified buil plan
        /// </summary>
        /// <param name="bp">Build plan</param>
        /// <returns>Created instance (throw exception if it can't be built).</returns>
        protected abstract object GetInstance(IBuildPlan bp);

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>objects which implements the service (or an empty list).</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            var bps = GetBuildPlans(typeof (T));

            var services = new T[bps.Count];
            for (var i = 0; i < services.Length; i++)
            {
                services[i] = (T) GetInstance(bps[i]);
            }

            return services;
        }

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <param name="service">Service to find</param>
        /// <returns>objects which implements the service (or an empty list).</returns>
        public IEnumerable<object> ResolveAll(Type service)
        {
            var bps = GetBuildPlans(service);

            var services = new object[bps.Count];
            for (var i = 0; i < services.Length; i++)
            {
                services[i] = GetInstance(bps[i]);
            }

            return services;
        }
    }
}