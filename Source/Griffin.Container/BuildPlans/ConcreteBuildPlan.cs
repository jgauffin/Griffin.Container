using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// A plan telling how concrete classes should be built.
    /// </summary>
    public class ConcreteBuildPlan : IBuildPlan
    {
        private readonly Type _concreteType;
        private readonly IInstanceStrategy _instanceStrategy;
        private ObjectActivator _factoryMethod;
        private ConstructorParameter[] _parameters;
        private ICreateCallback _createCallback;

        private class ConstructorParameter
        {
            public Type ServiceType { get; set; }
            public IBuildPlan BuildPlan { get; set; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ConcreteBuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="services">Services that the concrete implements </param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="instanceStrategy">Used to either fetch or create an instance.</param>
        public ConcreteBuildPlan(Type concreteType, IEnumerable<Type> services, Lifetime lifetime, IInstanceStrategy instanceStrategy)
        {
            if( concreteType == null && instanceStrategy == null)
                throw new ArgumentException("Either concreteType or instanceStrategy must be specified.");

            Lifetime = lifetime;
            _concreteType = concreteType;
            _instanceStrategy = instanceStrategy;
            Services = services.ToArray();
        }

        /// <summary>
        /// Gets lifetime of object
        /// </summary>
        public Lifetime Lifetime { get; private set; }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        public string DisplayName
        {
            get { return ConcreteType.FullName; }
        }

        public void SetCreateCallback(ICreateCallback callback)
        {
            _createCallback = callback;
        }


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
        public virtual void SetConstructor(ConstructorInfo constructor)
        {
            Constructor = constructor;
            _parameters = new ConstructorParameter[Constructor.GetParameters().Length];
            _factoryMethod = GetCreateDelegate();
        }

        /// <summary>
        /// Used to create the delegate used to create the instance.
        /// </summary>
        /// <returns></returns>
        protected virtual ObjectActivator GetCreateDelegate()
        {
            return Constructor.GetActivator();
        }


        /// <summary>
        /// Add another constructor parameter plan
        /// </summary>
        /// <param name="index">Index of the constructor parameter</param>
        /// <param name="bp">Plan used to construct the parameter</param>
        public void AddConstructorPlan(int index, IBuildPlan bp)
        {
            if (bp == null) throw new ArgumentNullException("bp");
            if (index < 0 || index >= Constructor.GetParameters().Length)
                throw new ArgumentOutOfRangeException("index");

            _parameters[index] = new ConstructorParameter
                                     {
                                         BuildPlan = bp,
                                         ServiceType = Constructor.GetParameters()[index].ParameterType
                                     };
        }

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// If an existing or an new instance is returned.
        /// </returns>
        public virtual InstanceResult GetInstance(CreateContext context, out object instance)
        {
            var strategyContext = new InstanceStrategyContext(this, context, () => Create(context));
            var result = _instanceStrategy.GetInstance(strategyContext, out instance);
            if (result == InstanceResult.Created && _createCallback != null)
                instance = _createCallback.InstanceCreated(context, this, instance);

            return result;
        }

        /// <summary>
        /// gets services that the concrete implements.
        /// </summary>
        public Type[] Services { get; private set; }


        /// <summary>
        /// Assembles all argument services and creates instance
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <returns>Created instance.</returns>
        protected virtual object Create(CreateContext context)
        {
            var parameters = new object[_parameters.Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                var paramContext = context.Clone(_parameters[i].ServiceType);
                object instance;
                _parameters[i].BuildPlan.GetInstance(paramContext, out instance);
                parameters[i] = instance;
            }

            return Create(context, parameters);
        }

        /// <summary>
        /// Creates the actual instance
        /// </summary>
        /// <param name="arguments">Constructor arguments</param>
        /// <returns>Created instance.</returns>
        protected virtual object Create(CreateContext context, object[] arguments)
        {
            return  _factoryMethod(arguments);
            /*return _createCallback != null
                       ? _createCallback.InstanceCreated(context, instance)
                       : instance;*/
        }

        #region Nested type: InstanceStrategyContext

        private class InstanceStrategyContext : IConcreteInstanceStrategyContext
        {
            private readonly ConcreteBuildPlan _bp;
            private readonly Func<object> _factory;

            public InstanceStrategyContext(ConcreteBuildPlan bp, CreateContext createContext, Func<object> factory)
            {
                _bp = bp;
                _factory = factory;
                CreateContext = createContext;
            }

            #region IInstanceStrategyContext Members

            public IBuildPlan BuildPlan
            {
                get { return _bp; }
            }

            public CreateContext CreateContext { get; private set; }


            public object CreateInstance()
            {
                return _factory();
            }

            #endregion
        }

        #endregion
    }
}