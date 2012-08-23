using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

namespace Griffin.Container.WebApi
{
    /// <summary>
    /// WebApi implementation
    /// </summary>
    /// <remarks>
    /// <example>
    /// You can get the current scope in this way:
    /// <code>
    /// <![CDATA[
    /// var scopeTracker = (IDependencyScopeTracker)GlobalConfiguration.Configuration.DependencyResolver;
    /// var currentScope = scopeTracker.Current;
    /// var someService = currentScope.Resolve<ISomeScopedService>();
    /// ]]>
    /// </code>
    /// </example>
    /// </remarks>
    /// <example>
    /// <code>
    /// var registrar = new ContainerRegistrar();
    /// 
    /// // Sample registration of business classes
    /// registrar.RegisterModules(typeof(SomeRepositoryClass).Assembly);
    /// 
    /// // the api controllers
    /// registrar.RegisterApiControllers(typeof(WebApiApplication).Assembly);
    ///
    /// // build the container
    /// _container = registrar.Build();
    /// 
    /// // Configure Dependency Injection in WebApi
    /// GlobalConfiguration.Configuration.DependencyResolver = new GriffinWebApiDependencyResolver(_container);
    /// </code>
    /// </example>
    public class GriffinWebApiDependencyResolver : IDependencyResolver, IDependencyScopeTracker
    {
        [ThreadStatic] private static Scope _currentChild;
        private readonly IParentContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="GriffinWebApiDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public GriffinWebApiDependencyResolver(IParentContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            _container = container;
        }

        #region IDependencyResolver Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        /// <summary>
        /// Get a service
        /// </summary>
        /// <param name="serviceType">Type of service to locate</param>
        /// <returns>Implementation if found; otherwise <c>null</c>.</returns>
        public object GetService(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            if (!_container.IsRegistered(serviceType))
                return null;

            return _container.Resolve(serviceType);
        }

        /// <summary>
        /// Find all matching implementations
        /// </summary>
        /// <param name="serviceType">Type of service to locate</param>
        /// <returns>Implementations if found; otherwise an empty collection.</returns>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");
            return _container.ResolveAll(serviceType);
        }

        /// <summary>
        /// Begins the scope.
        /// </summary>
        /// <returns></returns>
        public IDependencyScope BeginScope()
        {
            var child = _container.CreateChildContainer();
            _currentChild = new Scope(child, () => _currentChild = null);
            foreach (var startable in child.ResolveAll<IScopedStartable>())
            {
                startable.StartScoped();
            }

            return _currentChild;
        }

        #endregion

        #region IDependencyScopeTracker Members

        /// <summary>
        /// Gets the current threads dependency scope
        /// </summary>
        public IDependencyScope Current
        {
            get { return _currentChild; }
        }

        #endregion
    }
}