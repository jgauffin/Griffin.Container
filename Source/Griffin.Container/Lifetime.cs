namespace Griffin.Container
{
    /// <summary>
    /// Lifetime of an object
    /// </summary>
    public enum Lifetime
    {
        /// <summary>
        /// Default lifetime, currently scoped unless another lifetime is specified in the method call if <see cref="IContainerBuilder"/>.
        /// </summary>
        Default,

        /// <summary>
        /// Return a new object each time the service is request.
        /// </summary>
        Transient,

        /// <summary>
        /// Lives inside a <see cref="IChildContainer"/> and is disposed when the child container is disposed.
        /// </summary>
        Scoped,

        /// <summary>
        /// Same instance for the entire application lifetime.
        /// </summary>
        Singleton
    }
}