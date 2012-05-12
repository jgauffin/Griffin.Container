using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container
{
    /// <summary>
    /// Used to create a storage where all instances are saved.
    /// </summary>
    public interface IInstanceStorageFactory
    {
        /// <summary>
        /// Create storage used for the parent container
        /// </summary>
        /// <returns>Storage</returns>
        IInstanceStorage CreateParent();

        /// <summary>
        /// Create storage used for a scoped/child container.
        /// </summary>
        /// <returns>Storage</returns>
        IInstanceStorage CreateScoped();
    }
}
