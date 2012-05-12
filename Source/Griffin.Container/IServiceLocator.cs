using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// All containers implement the service locator pattern.
    /// </summary>
    public interface IServiceLocator
    {
        /// <summary>
        /// Checks if a component have been registered.
        /// </summary>
        /// <param name="type">Service which is requested.</param>
        /// <returns>true if registered; otherwise false.</returns>
        bool IsRegistered(Type type);

        /// <summary>
        /// Resolve a service.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>object which implements the service.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolve a service
        /// </summary>
        /// <param name="service">Requested service</param>
        /// <returns>object which implements the service</returns>
        object Resolve(Type service);

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <typeparam name="T">Requested service</typeparam>
        /// <returns>objects which implements the service (or an empty list).</returns>
        IEnumerable<T> ResolveAll<T>() where T : class;

        /// <summary>
        /// Resolve all found implementations.
        /// </summary>
        /// <param name="service">Service to find</param>
        /// <returns>objects which implements the service (or an empty list).</returns>
        IEnumerable<object> ResolveAll(Type service);
    }
}