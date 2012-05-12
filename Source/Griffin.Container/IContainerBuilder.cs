namespace Griffin.Container
{
    /// <summary>
    /// Used to build the container.
    /// </summary>
    public interface IContainerBuilder
    {
        /// <summary>
        /// Builds a container using the specified registrations.
        /// </summary>
        /// <param name="registrar">Registrations to use</param>
        /// <returns>A created container.</returns>
        IParentContainer Build(IContainerRegistrar registrar);
    }
}