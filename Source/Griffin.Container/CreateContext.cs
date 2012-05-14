using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Context used to create instances.
    /// </summary>
    public class CreateContext
    {
        private readonly LinkedList<ConcreteBuildPlan> _plansVisited = new LinkedList<ConcreteBuildPlan>();

        /// <summary>
        /// Gets or sets container
        /// </summary>
        public IServiceLocator Container { get; set; }

        /// <summary>
        /// Gets or sets singleton storage
        /// </summary>
        public IInstanceStorage Singletons { get; set; }

        /// <summary>
        /// Gets or set scoped storage
        /// </summary>
        public IInstanceStorage Scoped { get; set; }

        /// <summary>
        /// Add a build plan
        /// </summary>
        /// <param name="plan">build plan</param>
        public void Add(ConcreteBuildPlan plan)
        {
            if (plan == null) throw new ArgumentNullException("plan");
            if (_plansVisited.Contains(plan))
                throw new CircularDependenciesException("Circular dependencies.", _plansVisited);

            _plansVisited.AddLast(plan);
        }
    }
}