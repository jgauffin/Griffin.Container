namespace Griffin.Container
{
    /// <summary>
    /// Modules can be used to let each part of your system register it's on services.
    /// </summary>
    /// <remarks>Use <see cref="IContainerRegistrar.RegisterModules"/> to load all modules.</remarks>
    public interface IContainerModule
    {
        /// <summary>
        /// Register all services
        /// </summary>
        /// <param name="registrar">Registrar used for the registration</param>
        void Register(IContainerRegistrar registrar);
    }
}