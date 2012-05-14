using System;
using System.Reflection;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Used to build a component
    /// </summary>
    public class BuildPlan
    {
        private readonly Type _concreteType;
        private readonly IInstanceStrategy _instanceStrategy;
        private ObjectActivator _factoryMethod;
        private BuildPlan[] _parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="instanceStrategy">Used to either fetch or create an instance.</param>
        public BuildPlan(Type concreteType, Lifetime lifetime, IInstanceStrategy instanceStrategy)
        {
            if( concreteType == null && instanceStrategy == null)
                throw new ArgumentException("Either concreteType or instanceStrategy must be specified.");

            Lifetime = lifetime;
            _concreteType = concreteType;
            _instanceStrategy = instanceStrategy;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="instanceStrategy">Used to determine if a new instance or a stored one should be returned.</param>
        public BuildPlan(Type concreteType, IInstanceStrategy instanceStrategy)
        {
            if (concreteType == null && instanceStrategy == null)
                throw new ArgumentException("Either concreteType or instanceStrategy must be specified.");
            _concreteType = concreteType;
            _instanceStrategy = instanceStrategy;
        }

        /// <summary>
        /// Gets lifetime of object
        /// </summary>
        public Lifetime Lifetime { get; private set; }


        /// <summary>
        /// Gets the constructor which was chosen
        /// </summary>
        public ConstructorInfo Constructor { get; private set; }

        /// <summary>
        /// Gets type which should be created.
        /// </summary>
        /// <remarks>Might be null if an instanceStrategy have been specified in the BuildPlan constructor.</remarks>
        public Type ConcreteType
        {
            get { return _concreteType; }
        }

        /// <summary>
        /// Sets the constructor to use
        /// </summary>
        /// <param name="constructor"></param>
        public void SetConstructor(ConstructorInfo constructor)
        {
            Constructor = constructor;
            _parameters = new BuildPlan[Constructor.GetParameters().Length];
            _factoryMethod = constructor.GetActivator();
        }


        /// <summary>
        /// Add another constructor parameter plan
        /// </summary>
        /// <param name="index">Index of the constructor parameter</param>
        /// <param name="bp">Plan used to construct the parameter</param>
        public void AddConstructorPlan(int index, BuildPlan bp)
        {
            _parameters[index] = bp;
        }

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <returns>Instance if found; otherwise null.</returns>
        public virtual object GetInstance(CreateContext context)
        {
            var strategyContext = new InstanceStrategyContext(this, () => Create(context))
                                      {
                                          ScopedStorage = context.Scoped,
                                          SingletonStorage = context.Singletons,
                                          Container = context.Container
                                      };
            return _instanceStrategy.GetInstance(strategyContext);
        }


        /// <summary>
        /// Construct a new object
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <returns>Created instance.</returns>
        protected virtual object Create(CreateContext context)
        {
            context.Add(this);

            var parameters = new object[Constructor.GetParameters().Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                parameters[i] = _parameters[i].GetInstance(context);
            }

            return _factoryMethod(parameters);
        }

        #region Nested type: InstanceStrategyContext

        private class InstanceStrategyContext : IInstanceStrategyContext
        {
            private readonly BuildPlan _bp;
            private readonly Func<object> _factory;

            public InstanceStrategyContext(BuildPlan bp, Func<object> factory)
            {
                _bp = bp;
                _factory = factory;
            }

            #region IInstanceStrategyContext Members

            public BuildPlan BuildPlan
            {
                get { return _bp; }
            }

            public IInstanceStorage SingletonStorage { get; set; }

            public IInstanceStorage ScopedStorage { get; set; }

            public IServiceLocator Container { get; set; }

            public object CreateInstance()
            {
                return _factory();
            }

            #endregion
        }

        #endregion
    }
}