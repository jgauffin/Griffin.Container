using System;

namespace Griffin.Container
{
    /// <summary>
    /// Filter used when the registrar is looking for the services that a class implement
    /// </summary>
    public interface IServiceFilter
    {
        /// <summary>
        /// Determines if a concrete can be registered as the specified type.
        /// </summary>
        /// <param name="service">Implemented service</param>
        /// <returns>true if the class should be registered as the specified service; otherwise false.</returns>
        bool CanRegisterAs(Type service);
    }
}