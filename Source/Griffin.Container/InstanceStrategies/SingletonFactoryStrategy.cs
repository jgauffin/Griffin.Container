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

        #endregion
    }
}