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
        /// <param name="container">The service locator.</param>
        /// <param name="singletonStorage">The singleton storage.</param>
        /// <param name="scopedStorage">The scoped.</param>
        /// <param name="requestedService">The requested service.</param>
        public CreateContext(IServiceLocator container, IInstanceStorage singletonStorage, IInstanceStorage scopedStorage, Type requestedService)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (singletonStorage == null) throw new ArgumentNullException("singletonStorage");
            if (requestedService == null) throw new ArgumentNullException("requestedService");
            Container = container;
            SingletonStorage = singletonStorage;
            ScopedStorage = scopedStorage;
            RequestedService = requestedService;
        }

        /// <summary>
        /// Gets or sets container
        /// </summary>
        public IServiceLocator Container { get; private set; }

        /// <summary>
        /// Gets or sets singleton storage
        /// </summary>
        public IInstanceStorage SingletonStorage { get; private set; }

        /// <summary>
        /// Gets or set scoped storage
        /// </summary>
        public IInstanceStorage ScopedStorage { get; private set; }

        /// <summary>
        /// Gets requested service.
        /// </summary>
        public Type RequestedService { get; private set; }

        /// <summary>
        /// Clone context, but use another service type
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public CreateContext Clone(Type serviceType)
        {
            return new CreateContext(Container, SingletonStorage, ScopedStorage, serviceType);
        }
    }
}