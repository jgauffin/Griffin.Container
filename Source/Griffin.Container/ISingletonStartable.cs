namespace Griffin.Container
{
    /// <summary>
    /// Will be invoked when a parent container is created.
    /// </summary>
    public interface ISingletonStartable
    {
        /// <summary>
        /// Invoked when the parent container is created.
        /// </summary>
        void StartSingleton();
    }
}