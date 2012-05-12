namespace Griffin.Container
{
    /// <summary>
    /// Create a <see cref="InstanceStorage"/> class.
    /// </summary>
    public class DefaultInstanceStorageFactory : IInstanceStorageFactory
    {
        #region IInstanceStorageFactory Members

        /// <summary>
        /// Create storage used for the parent container
        /// </summary>
        /// <returns>Storage</returns>
        public IInstanceStorage CreateParent()
        {
            return new InstanceStorage();
        }

        /// <summary>
        /// Create storage used for a scoped/child container.
        /// </summary>
        /// <returns>Storage</returns>
        public IInstanceStorage CreateScoped()
        {
            return new InstanceStorage();
        }

        #endregion
    }
}