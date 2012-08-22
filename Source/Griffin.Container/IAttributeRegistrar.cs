using System;

namespace Griffin.Container
{
    /// <summary>
    /// Let your custom attribute configure the container
    /// </summary>
    /// <remarks>Used together <see cref="IContainerRegistrar.RegisterUsingAttribute{T}"/>. Your custom attribute should implement this interface.</remarks>
    public interface IAttributeRegistrar
    {
        /// <summary>
        /// Register the class in the container
        /// </summary>
        /// <param name="concrete">Class which has been decorated with the attribute.</param>
        /// <param name="registrar">The container registrar.</param>
        void Register(Type concrete, IContainerRegistrar registrar);
    }
}