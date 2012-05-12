using System;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Used to fetch or created scoped objects
    /// </summary>
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
        /// <returns>Created/Existing instance.</returns>
        public object GetInstance(IInstanceStrategyContext context)
        {
            if (context.ScopedStorage == null)
                throw new InvalidOperationException("Class '" + _concrete.FullName +
                                                    "' is a scoped object and can therefore not be created from the parent container.");

            var existing = context.ScopedStorage.Retreive(context.BuildPlan);
            if (existing != null)
                return existing;

            existing = context.CreateInstance();

            context.ScopedStorage.Store(context.BuildPlan, existing);
            return existing;
        }

        #endregion
    }
}