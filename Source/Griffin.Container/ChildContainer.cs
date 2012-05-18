﻿using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// A child container
    /// </summary>
    public class ChildContainer : ContainerBase, IChildContainer
    {
        private readonly IInstanceStorage _childStorage;
        private readonly Action _disposedCallback;
        private readonly IInstanceStorage _parentStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildContainer"/> class.
        /// </summary>
        /// <param name="serviceMappings">The service mappings.</param>
        /// <param name="parentStorage">The parent storage.</param>
        /// <param name="childStorage">The child storage.</param>
        /// <param name="disposedCallback">Invoked when the container is disposed.</param>
        public ChildContainer(IServiceMappings serviceMappings, IInstanceStorage parentStorage,
                              IInstanceStorage childStorage, Action disposedCallback) : base(serviceMappings)
        {
            _parentStorage = parentStorage;
            _childStorage = childStorage;
            _disposedCallback = disposedCallback;
        }

        #region IChildContainer Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            _childStorage.Dispose();
            if (_disposedCallback != null)
                _disposedCallback();
        }

        #endregion

        /// <summary>
        /// Get instance for the specified buil plan
        /// </summary>
        /// <param name="bp">Build plan</param>
        /// <returns>Created instance (throw exception if it can't be built).</returns>
        protected override object GetInstance(IBuildPlan bp)
        {
            var context = new CreateContext {Container = this, Scoped = _childStorage, Singletons = _parentStorage};
            return bp.GetInstance(context);
        }
    }
}