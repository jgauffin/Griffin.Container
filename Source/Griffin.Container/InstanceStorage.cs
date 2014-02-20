using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Stores created instances.
    /// </summary>
    public class InstanceStorage : IInstanceStorage
    {
        private readonly Dictionary<object, object> _items = new Dictionary<object, object>();

        #region IInstanceStorage Members

        /// <summary>
        /// Store a new instance
        /// </summary>
        /// <param name="key">Key identifying the instance</param>
        /// <param name="instance">Instance to store</param>
        public void Store(object key, object instance)
        {
            _items[key] = instance;
        }

        /// <summary>
        /// Fetch a stored instance
        /// </summary>
        /// <param name="key">Key identifying the instance</param>
        /// <returns>instance if found; otherwise null.</returns>
        public object Retrieve(object key)
        {
            object value;
            return _items.TryGetValue(key, out value) ? value : null;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            foreach (var value in _items.Values)
            {
                //do not implicitly dispose containers.
                if (value is IChildContainer || value is ContainerBase)
                    continue;

                var disposable = value as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            _items.Clear();
        }

        #endregion
    }
}