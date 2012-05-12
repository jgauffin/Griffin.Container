using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// Used to register classes in the container.
    /// </summary>
    public interface IContainerRegistrar
    {
        /// <summary>
        /// Gets all added registrations.
        /// </summary>
        IEnumerable<ComponentRegistration> Registrations { get; }

        /// <summary>
        /// Register classes which is decorated with the <see cref="ComponentAttribute"/>
        /// </summary>
        /// <param name="defaultLifetime">Lifetime to use if not specified in the <see cref="ComponentAttribute"/>.</param>
        /// <param name="path">File path to load assemblies from.</param>
        /// <param name="filePattern">File pattern to search for, same as for <see cref="Directory.GetFiles(string,string)"/>.</param>
        void RegisterComponents(Lifetime defaultLifetime, string path, string filePattern);

        /// <summary>
        /// Register classes which is decorated with the <see cref="ComponentAttribute"/>
        /// </summary>
        /// <param name="defaultLifetime">Lifetime to use if not specified in the <see cref="ComponentAttribute"/>.</param>
        /// <param name="assemblies">Assemblies to scan after the attribute</param>
        void RegisterComponents(Lifetime defaultLifetime, params Assembly[] assemblies);

        /// <summary>
        /// Register services using <see cref="IContainerModule"/> implementations.
        /// </summary>
        /// <param name="path">File path to load assemblies from.</param>
        /// <param name="filePattern">File pattern to search for, same as for <see cref="Directory.GetFiles(string, string)"/>.</param>
        void RegisterModules(string path, string filePattern);

        /// <summary>
        /// Register services using <see cref="IContainerModule"/> implementations.
        /// </summary>
        /// <param name="assemblies">Assemblies to scan after module implementations</param>
        void RegisterModules(params Assembly[] assemblies);

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        void RegisterType<TService>(Lifetime lifetime = Lifetime.Scoped) where TService : class;


        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <typeparam name="TConcrete">Object which will be constructed and returned.</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        void RegisterType<TService, TConcrete>(Lifetime lifetime = Lifetime.Scoped)
            where TService : class
            where TConcrete : class;

        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="service">Type which will be requested</param>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        void RegisterType(Type service, Lifetime lifetime = Lifetime.Scoped);

        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="service">Type which will be requested</param>
        /// <param name="clazz">Class which will be constructed and returned.</param>
        /// <param name="lifetime">Lifetime of the object that implements the service</param>
        void RegisterType(Type service, Type clazz, Lifetime lifetime = Lifetime.Scoped);

        /// <summary>
        /// Register an singleton
        /// </summary>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <param name="instance">Object which will be returned</param>
        void RegisterInstance<TService>(TService instance) where TService : class;

        /// <summary>
        /// Register an singleton
        /// </summary>
        /// <param name="service">Type which will be requested</param>
        /// <param name="clazz">Object which will be returned</param>
        void RegisterInstance(Type service, object clazz);
    }
}