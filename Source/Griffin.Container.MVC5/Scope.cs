using System;
using System.Collections.Generic;
using System.Security;
using System.Web.Http.Dependencies;

namespace Griffin.Container.Mvc5
{
    /// <summary>
    /// Our dependency scope implementation
    /// </summary>
    internal class Scope : IDependencyScope
    {
        private readonly IChildContainer _container;
        private readonly Action _disposeAction;
        private bool _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Scope"/> class.
        /// </summary>
        /// <param name="container">The scoped container.</param>
        /// <param name="disposeAction">The dispose action (invoked when the scope is disposed).</param>
        public Scope(IChildContainer container, Action disposeAction)
        {
            _container = container;
            _disposeAction = disposeAction;
        }


          /// <summary>
        /// Finalizes an instance of the <see cref="Scope"/> class.
        /// </summary>
        [SecuritySafeCritical]
        ~Scope()
        {
            Dispose(false);
        }

        #region IDependencyScope Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SecuritySafeCritical]
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_container != null)
                    {
                        _container.Dispose();
                        _disposeAction();
                    }
                }
                _disposed = true;
            }
        }
        
        public object GetService(Type serviceType)
        {
            if (!_container.IsRegistered(serviceType))
                return null;

            return _container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _container.ResolveAll(serviceType);
        }

        #endregion
    }
}