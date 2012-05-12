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
        /// <returns>Created/Existing instance.</returns>
        public object GetInstance(IInstanceStrategyContext context)
        {
            switch (_lifetime)
            {
                case Lifetime.Transient:
                    return _factory(context.Container);
                case Lifetime.Singleton:
                    return GetSingleton(context);
                case Lifetime.Scoped:
                    return GetScoped(context);
            }

            throw new NotSupportedException(string.Format("Lifetime not supported: {0}.", _lifetime));
        }

        #endregion

        private object GetSingleton(IInstanceStrategyContext context)
        {
            var existing = context.SingletonStorage.Retreive(context.BuildPlan);
            if (existing != null)
                return existing;

            existing = context.CreateInstance();
            context.SingletonStorage.Store(context.BuildPlan, existing);
            return existing;
        }

        private object GetScoped(IInstanceStrategyContext context)
        {
            if (context.ScopedStorage == null)
                throw new NotSupportedException("Scoped registrations need a scoped container.");

            var existing = context.ScopedStorage.Retreive(context.BuildPlan);
            if (existing != null)
                return existing;

            existing = context.CreateInstance();
            context.ScopedStorage.Store(context.BuildPlan, existing);
            return existing;
        }
    }
}