namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Return the same instance every time.
    /// </summary>
    public class SingletonFactoryStrategy : IInstanceStrategy
    {
        #region IInstanceStrategy Members

        /// <summary>
        /// Get instance.
        /// </summary>
        /// <param name="context">Information used to create/fetch instance.</param>
        /// <returns>Created/Existing instance.</returns>
        public object GetInstance(IInstanceStrategyContext context)
        {
            var existing = context.SingletonStorage.Retreive(context.BuildPlan);
            if (existing != null)
                return existing;

            existing = context.CreateInstance();

            context.SingletonStorage.Store(context.BuildPlan, existing);

            return existing;
        }

        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        public bool IsInstanceFactory
        {
            get { return false; }
        }

        #endregion
    }
}