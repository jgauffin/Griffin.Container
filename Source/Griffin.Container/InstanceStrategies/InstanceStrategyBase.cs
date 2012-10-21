using System;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Using an delegate to create the instance.
    /// </summary>
    public abstract class InstanceStrategyBase : IInstanceStrategy
    {
        private readonly Lifetime _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="InstanceStrategyBase"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        protected InstanceStrategyBase(Lifetime lifetime)
        {
            _lifetime = lifetime;
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
            switch (_lifetime)
            {
                case Lifetime.Transient:
                    instance = CreateInstance(context);
                    return InstanceResult.Created;
                case Lifetime.Singleton:
                    return GetSingleton(context, out instance);
                case Lifetime.Scoped:
                    return GetScoped(context, out instance);
            }

            throw new NotSupportedException(string.Format("Lifetime not supported: {0}.", _lifetime));
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Created instance</returns>
        protected abstract object CreateInstance(IInstanceStrategyContext context);


        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        public abstract bool IsInstanceFactory { get; }

        #endregion

        /// <summary>
        /// Gets the service as a singleton (created or from the storage)
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        protected InstanceResult GetSingleton(IInstanceStrategyContext context, out object instance)
        {
            instance = context.CreateContext.SingletonStorage.Retreive(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            instance = CreateInstance(context);
            context.CreateContext.SingletonStorage.Store(context.BuildPlan, instance);
            return InstanceResult.Created;
        }

        /// <summary>
        /// Gets the scoped object (created or from storage).
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        protected InstanceResult GetScoped(IInstanceStrategyContext context, out object instance)
        {
            if (context.CreateContext.ScopedStorage == null)
                throw new NotSupportedException("Scoped registrations need a scoped container. Requested service: " + context.BuildPlan.DisplayName);

            instance = context.CreateContext.ScopedStorage.Retreive(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            instance = CreateInstance(context);

            context.CreateContext.ScopedStorage.Store(context.BuildPlan, instance);
            return InstanceResult.Created;
        }
    }
}