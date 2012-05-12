namespace Griffin.Container
{
    /// <summary>
    /// Will be invoked when a scoped container is created.
    /// </summary>
    public interface IScopedStartable
    {
        /// <summary>
        /// Invoked when the scoped container is created.
        /// </summary>
        void StartScoped();
    }

    /// <summary>
    /// Will be invoked when a parent container is created.
    /// </summary>
    public interface ISingletonStartable
    {
        /// <summary>
        /// Invoked when the parent container is created.
        /// </summary>
        void StartScoped();
    }
}