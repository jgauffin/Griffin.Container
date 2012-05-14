using System;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Maps a service to a build plan which is generated outside.
    /// </summary>
    public class ExternalBuildPlan : IBuildPlan
    {
        private readonly Lifetime _lifetime;
        private readonly IInstanceStrategy _instanceStrategy;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalBuildPlan"/> class.
        /// </summary>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="instanceStrategy">The instance strategy.</param>
        public ExternalBuildPlan(Lifetime lifetime, IInstanceStrategy instanceStrategy)
        {
            if (instanceStrategy == null) throw new ArgumentNullException("instanceStrategy");
            _lifetime = lifetime;
            _instanceStrategy = instanceStrategy;
        }

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <returns>Instance if found; otherwise null.</returns>
        public object GetInstance(CreateContext context)
        {
            var strategyContext = new ExternalInstanceStrategyContext
            {
                ScopedStorage = context.Scoped,
                SingletonStorage = context.Singletons,
                Container = context.Container
            };

            return _instanceStrategy.GetInstance(strategyContext);
        }

        class ExternalInstanceStrategyContext : IInstanceStrategyContext
        {
            public ConcreteBuildPlan BuildPlan { get;  set; }
            public IInstanceStorage SingletonStorage { get;  set; }
            public IInstanceStorage ScopedStorage { get;  set; }
            public IServiceLocator Container { get;  set; }
            public object CreateInstance()
            {
                throw new NotSupportedException("Create the instance yourself, huh.");
            }
        }
    }
}