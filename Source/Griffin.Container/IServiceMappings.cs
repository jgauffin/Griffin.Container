using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Griffin.Container.BuildPlans;

namespace Griffin.Container
{
    /// <summary>
    /// Maps services to build plans
    /// </summary>
    public interface IServiceMappings : IEnumerable<KeyValuePair<Type, IEnumerable<IBuildPlan>>>
    {
        /// <summary>
        /// Checks if the specified service exists.
        /// </summary>
        /// <param name="service">Service to check</param>
        /// <returns>true if found; otherwise false.</returns>
        bool Contains(Type service);

        /// <summary>
        /// Try get build plans for a service.
        /// </summary>
        /// <param name="service">Service to get plans for</param>
        /// <param name="buildPlans">Found plans</param>
        /// <returns>true if found; otherwise false.</returns>
        bool TryGetValue(Type service, out IList<IBuildPlan> buildPlans);

        /// <summary>
        /// Add a new service
        /// </summary>
        /// <param name="service">Service to add</param>
        /// <param name="buildPlans">Build plans for the service.</param>
        void Add(Type service, IList<IBuildPlan> buildPlans);
    }
}
