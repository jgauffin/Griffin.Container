using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Griffin.Container.Mvc4
{
    /// <summary>
    /// Griffin.Container implementation
    /// </summary>
    public class GriffinDependencyResolver : IDependencyResolver
    {
        [ThreadStatic] private static IChildContainer _childContainer;
        private readonly IParentContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="GriffinDependencyResolver"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public GriffinDependencyResolver(IParentContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");
            _container = container;
        }

        /// <summary>
        /// Gets current child container
        /// </summary>
        protected IChildContainer ChildContainer
        {
            get
            {

                return _childContainer ?? (_childContainer = CreateAndStartChildContainer());
            }
        }

        private IChildContainer CreateAndStartChildContainer()
        {
            var child = _container.CreateChildContainer();
            child.Resolve<IScopedStartable>().StartScoped();
            return child;
        }

        #region IDependencyResolver Members

        /// <summary>
        /// Resolves singly registered services that support arbitrary object creation.
        /// </summary>
        /// <returns>
        /// The requested service or object.
        /// </returns>
        /// <param name="serviceType">The type of the requested service or object.</param>
        public object GetService(Type serviceType)
        {
            return !ChildContainer.IsRegistered(serviceType) ? null : ChildContainer.Resolve(serviceType);
        }

        /// <summary>
        /// Resolves multiply registered services.
        /// </summary>
        /// <returns>
        /// The requested services.
        /// </returns>
        /// <param name="serviceType">The type of the requested services.</param>
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return !ChildContainer.IsRegistered(serviceType) ? new object[0] : ChildContainer.ResolveAll(serviceType);
        }

        #endregion

        /// <summary>
        /// Dispose current child container (if any)
        /// </summary>
        public static void DisposeChildContainer()
        {
            if (_childContainer == null)
                return;

            _childContainer.Dispose();
            _childContainer = null;
        }
    }
}