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
        /// Gets or sets container
        /// </summary>
        public IServiceLocator Container { get; set; }

        /// <summary>
        /// Gets or sets singleton storage
        /// </summary>
        public IInstanceStorage Singletons { get; set; }

        /// <summary>
        /// Gets or set scoped storage
        /// </summary>
        public IInstanceStorage Scoped { get; set; }
    }
}