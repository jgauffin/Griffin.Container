using System;

namespace Griffin.Container
{
    /// <summary>
    /// Used to store instances
    /// </summary>
    public interface IInstanceStorage : IDisposable
    {
        /// <summary>
        /// Store a new isntance
        /// </summary>
        /// <param name="key">Key identifying the instance</param>
        /// <param name="instance">Instance to store</param>
        void Store(object key, object instance);

        /// <summary>
        /// Fetch a stored instance
        /// </summary>
        /// <param name="key">Key identifying the instance</param>
        /// <returns>instance if found; otherwise null.</returns>
        object Retrieve(object key);
    }
}