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
    }
}