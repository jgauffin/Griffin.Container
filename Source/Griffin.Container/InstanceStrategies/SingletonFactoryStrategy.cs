namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Return the same instance every time.
    /// </summary>
    /// <remarks>Requires a <see cref="IConcreteInstanceStrategyContext"/></remarks>
    public class SingletonFactoryStrategy : IInstanceStrategy
    {
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
            instance = context.CreateContext.SingletonStorage.Retrieve(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            lock (context.CreateContext.SingletonStorage)
            {
                instance = context.CreateContext.SingletonStorage.Retrieve(context.BuildPlan);
                if (instance != null)
                    return InstanceResult.Loaded;

                var ctx = (IConcreteInstanceStrategyContext)context;
                instance = ctx.CreateInstance();
                context.CreateContext.SingletonStorage.Store(context.BuildPlan, instance);
            }

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