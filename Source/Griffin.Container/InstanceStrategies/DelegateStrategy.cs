using System;

namespace Griffin.Container.InstanceStrategies
{

    /// <summary>
    /// Using an delegate to create the instance.
    /// </summary>
    public class DelegateStrategy<T> : InstanceStrategyBase
    {
        private readonly Func<IServiceLocator, T> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateStrategy{T}"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        public DelegateStrategy(Func<IServiceLocator, T> factory, Lifetime lifetime)
            : base(lifetime)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _factory = factory;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Created instance</returns>
        protected override object CreateInstance(IInstanceStrategyContext context)
        {
            return _factory(context.CreateContext.Container);
        }

        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        public override bool IsInstanceFactory
        {
            get { return true; }
        }
    }

    /// <summary>
    /// Using an delegate to create the instance.
    /// </summary>
    public class DelegateStrategy : InstanceStrategyBase
    {
        private readonly Func<IServiceLocator, object> _factory;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateStrategy"/> class.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <param name="lifetime">The lifetime.</param>
        public DelegateStrategy(Func<IServiceLocator, object> factory, Lifetime lifetime)
            : base(lifetime)
        {
            if (factory == null) throw new ArgumentNullException("factory");
            _factory = factory;
        }

        /// <summary>
        /// Create a new instance.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>Created instance</returns>
        protected override object CreateInstance(IInstanceStrategyContext context)
        {
            return _factory(context.CreateContext.Container);
        }

        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        public override bool IsInstanceFactory
        {
            get { return true; }
        }
    }
}