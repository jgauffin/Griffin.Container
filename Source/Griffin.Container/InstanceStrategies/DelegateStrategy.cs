using System;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Using an delegate to create the instance.
    /// </summary>
    public class DelegateStrategy : IInstanceStrategy
    {
        private readonly Func<IServiceLocator, object> _factory;
        private readonly Lifetime _lifetime;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateStrategy"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        public DelegateStrategy(Func<IServiceLocator, object> factory, Lifetime lifetime)
        {
            _factory = factory;
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
                    instance = _factory(context.CreateContext.Container);
                    return InstanceResult.Created;
                case Lifetime.Singleton:
                    return GetSingleton(context, out instance);
                case Lifetime.Scoped:
                    return GetScoped(context, out instance);
            }

            throw new NotSupportedException(string.Format("Lifetime not supported: {0}.", _lifetime));
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

        private InstanceResult GetSingleton(IInstanceStrategyContext context, out object instance)
        {
            instance = context.CreateContext.SingletonStorage.Retreive(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            instance = _factory(context.CreateContext.Container);
            context.CreateContext.SingletonStorage.Store(context.BuildPlan, instance);
            return InstanceResult.Created;
        }

        private InstanceResult GetScoped(IInstanceStrategyContext context, out object instance)
        {
            if (context.CreateContext.ScopedStorage == null)
                throw new NotSupportedException("Scoped registrations need a scoped container.");

            instance = context.CreateContext.ScopedStorage.Retreive(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            instance = _factory(context.CreateContext.Container);

            context.CreateContext.ScopedStorage.Store(context.BuildPlan, instance);
            return InstanceResult.Created;
        }
    }
}