using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Maps services to build plans.
    /// </summary>
    public class ServiceMappings : IServiceMappings
    {
        private readonly Dictionary<Type, IList<IBuildPlan>> _services = new Dictionary<Type, IList<IBuildPlan>>();

        /// <summary>
        /// Checks if the specified service exists.
        /// </summary>
        /// <param name="service">Service to check</param>
        /// <returns>true if found; otherwise false.</returns>
        public bool Contains(Type service)
        {
            return _services.ContainsKey(service);
        }

        /// <summary>
        /// Try get build plans for a service.
        /// </summary>
        /// <param name="service">Service to get plans for</param>
        /// <param name="buildPlans">Found plans</param>
        /// <returns>true if found; otherwise false.</returns>
        public bool TryGetValue(Type service, out IList<IBuildPlan> buildPlans)
        {
            return _services.TryGetValue(service, out buildPlans);
        }

        /// <summary>
        /// Add a new service
        /// </summary>
        /// <param name="service">Service to add</param>
        /// <param name="buildPlans">Build plans for the service.</param>
        public void Add(Type service, IList<IBuildPlan> buildPlans)
        {
            _services.Add(service, buildPlans);
        }
    }
}