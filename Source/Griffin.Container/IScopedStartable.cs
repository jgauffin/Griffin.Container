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
}