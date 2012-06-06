using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Griffin.Container.BuildPlans;

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

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<Type, IEnumerable<IBuildPlan>>> GetEnumerator()
        {
            return _services.Select(service => new KeyValuePair<Type, IEnumerable<IBuildPlan>>(service.Key, service.Value)).GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}