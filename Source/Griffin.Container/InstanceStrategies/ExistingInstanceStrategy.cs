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
        /// <param name="instance">Instance that was loaded/created</param>
        /// <returns>
        /// If the instance was created or loaded from a storage.
        /// </returns>
        public InstanceResult GetInstance(IInstanceStrategyContext context, out object instance)
        {
            instance = _instance;
            return InstanceResult.Loaded;
        }

        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        public bool IsInstanceFactory
        {
            get { return true; }
        }

        #endregion
    }
}