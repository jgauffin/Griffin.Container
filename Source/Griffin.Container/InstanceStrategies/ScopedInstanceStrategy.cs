using System;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Used to fetch or created scoped objects
    /// </summary>
    /// <remarks>Requires a <see cref="IConcreteInstanceStrategyContext"/></remarks>
    public class ScopedInstanceStrategy : IInstanceStrategy
    {
        private readonly Type _concrete;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScopedInstanceStrategy"/> class.
        /// </summary>
        /// <param name="concrete">The concrete.</param>
        public ScopedInstanceStrategy(Type concrete)
        {
            _concrete = concrete;
        }

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
            if (context.CreateContext.ScopedStorage == null)
                throw new InvalidOperationException("Class '" + _concrete.FullName +
                                                    "' is a scoped object and can therefore not be created from the parent container.");

            instance = context.CreateContext.ScopedStorage.Retreive(context.BuildPlan);
            if (instance != null)
                return InstanceResult.Loaded;

            var ctx = (IConcreteInstanceStrategyContext)context;
            instance = ctx.CreateInstance();

            context.CreateContext.ScopedStorage.Store(context.BuildPlan, instance);
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