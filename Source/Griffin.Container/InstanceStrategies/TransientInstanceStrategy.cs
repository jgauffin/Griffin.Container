namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Create a new instance every time
    /// </summary>
    /// <remarks>Requires a <see cref="IConcreteInstanceStrategyContext"/></remarks>
    public class TransientInstanceStrategy : IInstanceStrategy
    {
        #region IInstanceStrategy Members

        /// <summary>
        /// Get instance.
        /// </summary>
        /// <param name="context">Information used to create/fetch instance.</param>
        /// <param name="instance">Instance that was loaded/created</param>
        /// <returns>
        /// Created/Existing instance.
        /// </returns>
        public InstanceResult GetInstance(IInstanceStrategyContext context, out object instance)
        {
            var ctx = (IConcreteInstanceStrategyContext)context;
            instance = ctx.CreateInstance();
            return InstanceResult.Created;
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