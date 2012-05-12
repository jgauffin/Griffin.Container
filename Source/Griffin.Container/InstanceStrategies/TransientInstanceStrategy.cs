namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Create a new instance every time
    /// </summary>
    public class TransientInstanceStrategy : IInstanceStrategy
    {
        #region IInstanceStrategy Members

        /// <summary>
        /// Get instance.
        /// </summary>
        /// <param name="context">Information used to create/fetch instance.</param>
        /// <returns>Created/Existing instance.</returns>
        public object GetInstance(IInstanceStrategyContext context)
        {
            return context.CreateInstance();
        }

        #endregion
    }
}