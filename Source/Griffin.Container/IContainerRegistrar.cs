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
        /// <typeparam name="TConcrete">Type to create</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        /// <remarks>Will be registered as all interfaces &amp; subclasses which is not rejected by the current <see cref="IServiceFilter"/>.</remarks>
        void RegisterConcrete<TConcrete>(Lifetime lifetime = Lifetime.Scoped) where TConcrete : class;

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TService">Services which is requested from the container.</typeparam>
        /// <param name="factory">Delegate used to produce the instance.</param>
        /// <param name="lifetime">Lifetime of the returned object</param>
        void RegisterService<TService>(Func<IServiceLocator, object> factory, Lifetime lifetime = Lifetime.Scoped);

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TConcrete">Object which will be constructed and returned.</typeparam>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        void RegisterType<TService, TConcrete>(Lifetime lifetime = Lifetime.Scoped)
            where TService : class
            where TConcrete : TService;

        /// <summary>
        /// Register a cpncrete
        /// </summary>
        /// <param name="concrete">Type which will be created</param>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        /// <remarks>Will be registered as all interfaces &amp; subclasses which is not rejected by the current <see cref="IServiceFilter"/>.</remarks>
        void RegisterConcrete(Type concrete, Lifetime lifetime = Lifetime.Scoped);

        /// <summary>
        /// Register a service.
        /// </summary>
        /// <param name="service">Services which is requested from the container.</param>
        /// <param name="factory">Delegate used to produce the instance.</param>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        void RegisterService(Type service, Func<IServiceLocator, object> factory, Lifetime lifetime = Lifetime.Scoped);

        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="concrete">Class which will be constructed and returned.</param>
        /// <param name="service">Type which will be requested</param>
        /// <param name="lifetime">Lifetime of the object that implements the service</param>
        void RegisterType(Type service, Type concrete, Lifetime lifetime = Lifetime.Scoped);

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
        /// <param name="concrete">Object which will be returned</param>
        void RegisterInstance(Type service, object concrete);


    }
}