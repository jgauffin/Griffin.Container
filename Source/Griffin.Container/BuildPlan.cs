using System;
using System.Reflection;
using System.Reflection.Emit;

namespace Griffin.Container
{
    /// <summary>
    /// Used to build a component
    /// </summary>
    public class BuildPlan
    {
        private readonly Type _concreteType;
        private readonly Func<IInstanceStorage, IInstanceStorage, object> _factory;
        private readonly object _instance;
        private BuildPlan[] _parameters;
        private ObjectActivator _factoryMethod;

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="lifetime">The lifetime.</param>
        public BuildPlan(Type concreteType, Lifetime lifetime)
        {
            if (concreteType == null) throw new ArgumentNullException("concreteType");
            _concreteType = concreteType;
            switch (lifetime)
            {
                case Lifetime.Singleton:
                    _factory = GetSingleton;
                    break;
                case Lifetime.Transient:
                    _factory = GetTransient;
                    break;
                case Lifetime.Scoped:
                    _factory = GetScoped;
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} is not supported.", lifetime));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="instance">Singleton.</param>
        public BuildPlan(Type concreteType, object instance)
        {
            if (concreteType == null) throw new ArgumentNullException("concreteType");
            _concreteType = concreteType;
            _instance = instance;
            _factory = (storage, instanceStorage) => instance;
        }


        /// <summary>
        /// Gets the constructor which was chosen
        /// </summary>
        public ConstructorInfo Constructor { get; private set; }

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
        /// <param name="singletons">Singleton storage</param>
        /// <param name="scoped">Scoped storage. Should be set to null when the parent container is used for service location.</param>
        /// <returns>Instance if found; otherwise null.</returns>
        public virtual object GetInstance(IInstanceStorage singletons, IInstanceStorage scoped)
        {
            return _factory(singletons, scoped);
        }

        /// <summary>
        /// Get object from singleton storage
        /// </summary>
        /// <param name="singletons">Storage for singletons (parent container)</param>
        /// <param name="scoped">Storage for scoped objects.</param>
        /// <returns>Object if found; otherwise null.</returns>
        protected virtual object GetSingleton(IInstanceStorage singletons, IInstanceStorage scoped)
        {
            var existing = singletons.Retreive(this);
            if (existing != null)
                return existing;

            existing = Create(singletons, scoped);

            singletons.Store(this, existing);
            return existing;
        }

        /// <summary>
        /// Get a new object each time
        /// </summary>
        /// <param name="singletons">Storage for singletons (parent container)</param>
        /// <param name="scoped">Storage for scoped objects.</param>
        /// <returns>Object</returns>
        protected virtual object GetTransient(IInstanceStorage singletons, IInstanceStorage scoped)
        {
            return Create(singletons, scoped);
        }

        /// <summary>
        /// Get object from the scoped storage
        /// </summary>
        /// <param name="singletons">Storage for singletons (parent container)</param>
        /// <param name="scoped">Storage for scoped objects.</param>
        /// <returns>Object if found; otherwise null.</returns>
        protected virtual object GetScoped(IInstanceStorage singletons, IInstanceStorage scoped)
        {
            if (scoped == null)
                throw new InvalidOperationException("Class '" + _concreteType.FullName + "' is a scoped object and can therefore not be created from the parent container.");

            var existing = scoped.Retreive(this);
            if (existing != null)
                return existing;

            existing = Create(singletons, scoped);

            scoped.Store(this, existing);
            return existing;
        }

        /// <summary>
        /// Construct a new object
        /// </summary>
        /// <param name="singletons">Storage for singletons (parent container)</param>
        /// <param name="scoped">Storage for scoped objects.</param>
        /// <returns>Created instance.</returns>
        protected virtual object Create(IInstanceStorage singletons, IInstanceStorage scoped)
        {
            var parameters = new object[Constructor.GetParameters().Length];
            for (var i = 0; i < parameters.Length; i++)
            {
                parameters[i] = _parameters[i].GetInstance(singletons, scoped);
            }

            return _factoryMethod(parameters);
        }
    }
}