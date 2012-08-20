using System;
using System.Collections.Generic;
using Griffin.Container.BuildPlans;

namespace Griffin.Container
{
    /// <summary>
    /// Base class for the container.
    /// </summary>
    public abstract class ContainerBase : IServiceLocator, ICreateCallback
    {
        private readonly IServiceMappings _serviceMappings;
        private List<IInstanceDecorator> _decorators = new List<IInstanceDecorator>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerBase"/> class.
        /// </summary>
        /// <param name="serviceMappings">The service mappings.</param>
        protected ContainerBase(IServiceMappings serviceMappings)
        {
            if (serviceMappings == null) throw new ArgumentNullException("serviceMappings");
            _serviceMappings = serviceMappings;
            foreach (var mapping in serviceMappings)
            {
                foreach (var concrete in mapping.Value)
                {
                    concrete.SetCreateCallback(this);
                }
            }
        }

        /// <summary>
        /// Get all service mappings
        /// </summary>
        protected IServiceMappings ServiceMappings
        {
            get { return _serviceMappings; }
        }

        /// <summary>
        /// Checks if a component have been registered.
        /// </summary>
        /// <param name="type">Service which is requested.</param>
        /// <returns>true if registered; otherwise false.</returns>
        public bool IsRegistered(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            return ServiceMappings.Contains(type);
        }

        /// <summary>
        /// Resolve a service.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>object which implements the service.</returns>
        public T Resolve<T>() where T : class
        {
            return (T) Resolve(typeof (T));
        }

        /// <summary>
        /// Resolve a service
        /// </summary>
        /// <param name="service">Requested service</param>
        /// <returns>object which implements the service</returns>
        public object Resolve(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            var bps = GetBuildPlans(service);
            if (bps == null)
                throw new ServiceNotRegisteredException(service);
            var instance = GetInstance(bps[0], service);
            if (instance == null)
                throw new InvalidOperationException(string.Format("Failed to construct '{0}'", service.FullName));

            return instance;
        }

        /// <summary>
        /// Gets the build plans for all concretes that implements a service.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <returns>Build plans</returns>
        protected virtual IList<IBuildPlan> GetBuildPlans(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            IList<IBuildPlan> bps;
            if (!ServiceMappings.TryGetValue(service, out bps))
            {
                if (service.IsGenericType)
                {
                    var genericService = service.GetGenericTypeDefinition();
                    if (!ServiceMappings.TryGetValue(genericService, out bps))
                    {
                        throw new InvalidOperationException(string.Format("Service {0} has not been registered.",
                                                                          service.FullName));
                    }
                }

            }
                

            return bps;
        }


        /// <summary>
        /// Gets storage for scoped objects.
        /// </summary>
        protected abstract IInstanceStorage ChildStorage { get; }

        /// <summary>
        /// Gets storage for singletons
        /// </summary>
        protected abstract IInstanceStorage RootStorage { get; }

        /// <summary>
        /// Get instance for the specified buil plan
        /// </summary>
        /// <param name="bp">Build plan describing how to create and store the object</param>
        /// <param name="requestedService">Service to build.</param>
        /// <returns>Created instance (throw exception if it can't be built).</returns>
        protected virtual object GetInstance(IBuildPlan bp, Type requestedService)
        {
            if (requestedService == null) throw new ArgumentNullException("requestedService");
            var context = new CreateContext(this, RootStorage, ChildStorage, requestedService);
            object instance;
            bp.GetInstance(context, out instance);
            return instance;
        }

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>objects which implements the service (or an empty list).</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            IList<IBuildPlan> bps;
            if (!ServiceMappings.TryGetValue(typeof(T), out bps))
                return new T[0];

            var services = new T[bps.Count];
            for (var i = 0; i < services.Length; i++)
            {
                services[i] = (T) GetInstance(bps[i], typeof(T));
            }

            return services;
        }

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <param name="service">Service to find</param>
        /// <returns>objects which implements the service (or an empty list).</returns>
        public IEnumerable<object> ResolveAll(Type service)
        {
            IList<IBuildPlan> bps;
            if (!ServiceMappings.TryGetValue(service, out bps))
                return new object[0];

            var services = new object[bps.Count];
            for (var i = 0; i < services.Length; i++)
            {
                services[i] = GetInstance(bps[i], service);
            }

            return services;
        }

        object ICreateCallback.InstanceCreated(CreateContext context, IBuildPlan buildPlan, object instance)
        {
            if (_decorators.Count == 0)
                return instance;

            var ctx = new DecoratorContext(buildPlan.Services, buildPlan.Lifetime)
                          {Instance = instance, RequestedService = context.RequestedService};
            foreach (var decorator in _decorators)
            {
                decorator.Decorate(ctx);
            }

            return ctx.Instance;
        }

        /// <summary>
        /// Add a new decorator
        /// </summary>
        /// <param name="decorator">Decorator to run</param>
        /// <remarks>Read about the decorator pattern to understand what these do. The interface is called for
        /// each new instance which is created.</remarks>
        public void AddDecorator(IInstanceDecorator decorator)
        {
            if (decorator == null) throw new ArgumentNullException("decorator");
            _decorators.Add(decorator);
        }
    }
}