using System;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Object has already been created.
    /// </summary>
    public class ExistingInstanceStrategy : IInstanceStrategy
    {
        private readonly object _instance;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExistingInstanceStrategy"/> class.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public ExistingInstanceStrategy(object instance)
        {
            if (instance == null) throw new ArgumentNullException("instance");
            _instance = instance;
        }

        #region IInstanceStrategy Members

        /// <summary>
        /// Get instance.
        /// </summary>
        /// <param name="context">Information used to create/fetch instance.</param>
        /// <returns>Created/Existing instance.</returns>
        public object GetInstance(IInstanceStrategyContext context)
        {
            return _instance;
        }

        #endregion
    }
}