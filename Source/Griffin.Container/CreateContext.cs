using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Context used to create instances.
    /// </summary>
    public class CreateContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateContext"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        /// <param name="singletonStorage">The singleton storage.</param>
        /// <param name="scopedStorage">The scoped.</param>
        /// <param name="requestedService">The requested service.</param>
        public CreateContext(IServiceLocator serviceLocator, IInstanceStorage singletonStorage, IInstanceStorage scopedStorage, Type requestedService)
        {
            Container = serviceLocator;
            Singletons = singletonStorage;
            Scoped = scopedStorage;
            RequestedService = requestedService;
        }

        /// <summary>
        /// Gets or sets container
        /// </summary>
        public IServiceLocator Container { get; private set; }

        /// <summary>
        /// Gets or sets singleton storage
        /// </summary>
        public IInstanceStorage Singletons { get; private set; }

        /// <summary>
        /// Gets or set scoped storage
        /// </summary>
        public IInstanceStorage Scoped { get; private set; }

        /// <summary>
        /// Gets requested service.
        /// </summary>
        public Type RequestedService { get; private set; }
    }
}