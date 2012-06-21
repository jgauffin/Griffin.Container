namespace Griffin.Container
{
    /// <summary>
    /// Parent container, holds all singletons
    /// </summary>
    public interface IParentContainer : IServiceLocator
    {
        /// <summary>
        /// Create a new child container (holds all scoped services)
        /// </summary>
        /// <returns>Child container.</returns>
        IChildContainer CreateChildContainer();

        /// <summary>
        /// Gets current child
        /// </summary>
        /// <remarks>Returns the last one created (in the current thread) if several child containers have been created-</remarks>
        IChildContainer CurrentChild { get; }
    }
}