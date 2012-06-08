using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Used to register all services which will be created by the container.
    /// </summary>
    public class ContainerRegistrar : IContainerRegistrar
    {
        private readonly List<ComponentRegistration> _registrations = new List<ComponentRegistration>();
        private readonly IServiceFilter _serviceFilter;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRegistrar"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public ContainerRegistrar(IServiceFilter filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            _serviceFilter = filter;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerRegistrar"/> class.
        /// </summary>
        public ContainerRegistrar()
        {
            _serviceFilter = new NonFrameworkClasses();
        }

        /// <summary>
        /// Builds the container directly.
        /// </summary>
        /// <returns>Generated container.</returns>
        public Container Build()
        {
            var builder = new ContainerBuilder();
            return (Container)builder.Build(this);
        }


        #region IContainerRegistrar Members

        /// <summary>
        /// Register classes which is decorated with the <see cref="ComponentAttribute"/>
        /// </summary>
        /// <param name="defaultLifetime">Lifetime to use if not specified in the <see cref="ComponentAttribute"/>.</param>
        /// <param name="path">File path to load assemblies from.</param>
        /// <param name="filePattern">File pattern to search for, same as for <see cref="Directory.GetFiles(string,string)"/>.</param>
        public void RegisterComponents(Lifetime defaultLifetime, string path, string filePattern)
        {
            if (path == null) throw new ArgumentNullException("path");
            if (filePattern == null) throw new ArgumentNullException("filePattern");
            if (defaultLifetime == Lifetime.Default)
                throw new ArgumentOutOfRangeException("defaultLifetime", "May not be set to Lifetime.Default.");


            foreach (var assembly in AssemblyUtils.LoadAssemblies(path, filePattern))
            {
                RegisterComponents(defaultLifetime, assembly);
            }
        }

        /// <summary>
        /// Register classes which is decorated with the <see cref="ComponentAttribute"/>
        /// </summary>
        /// <param name="defaultLifetime">Lifetime to use if not specified in the <see cref="ComponentAttribute"/>.</param>
        /// <param name="assemblies">Assemblies to scan after the attribute</param>
        public void RegisterComponents(Lifetime defaultLifetime, params Assembly[] assemblies)
        {
            if (assemblies == null) throw new ArgumentNullException("assemblies");
            if (defaultLifetime == Lifetime.Default)
                throw new ArgumentOutOfRangeException("defaultLifetime", "May not be set to Lifetime.Default.");

            var componentType = typeof (ComponentAttribute);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.IsInterface || type.IsAbstract)
                        continue;

                    var attr = type.GetCustomAttributes(componentType, true).Cast<ComponentAttribute>().FirstOrDefault();
                    if (attr == null)
                        continue;

                    var lifetime = attr.Lifetime == Lifetime.Default
                                       ? defaultLifetime
                                       : attr.Lifetime;
                    RegisterComponent(type, lifetime);
                }
            }
        }

        /// <summary>
        /// Gets all registrations.
        /// </summary>
        public IEnumerable<ComponentRegistration> Registrations
        {
            get { return _registrations; }
        }

        /// <summary>
        /// Register services using <see cref="IContainerModule"/> implementations.
        /// </summary>
        /// <param name="path">File path to load assemblies from.</param>
        /// <param name="filePattern">File pattern to search for, same as for <see cref="Directory.GetFiles(string, string)"/>.</param>
        public void RegisterModules(string path, string filePattern)
        {
            foreach (var assembly in AssemblyUtils.LoadAssemblies(path, filePattern))
            {
                RegisterModules(assembly);
            }
        }

        /// <summary>
        /// Register services using <see cref="IContainerModule"/> implementations.
        /// </summary>
        /// <param name="assemblies">Assemblies to scan after module implementations</param>
        public void RegisterModules(params Assembly[] assemblies)
        {
            var modType = typeof (IContainerModule);
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(modType.IsAssignableFrom))
                {
                    var module = (IContainerModule) Activator.CreateInstance(type);
                    module.Register(this);
                }
            }
        }

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TConcrete">Object to create. Register it's implemented interfaces</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        public void RegisterConcrete<TConcrete>(Lifetime lifetime = Lifetime.Scoped) where TConcrete : class
        {
            RegisterConcrete(typeof (TConcrete), lifetime);
        }

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TService">Requested service</typeparam>
        /// <param name="factory">Delegate used to produce the instance.</param>
        /// <param name="lifetime">Lifetime of the returned object</param>
        public void RegisterService<TService>(Func<IServiceLocator, object> factory, Lifetime lifetime = Lifetime.Scoped)
        {
            var registration = CreateRegistration(null, lifetime);
            registration.AddService(typeof (TService));
            registration.InstanceStrategy = new DelegateStrategy(factory, lifetime);
            Add(registration);
        }

        /// <summary>
        /// Register a type
        /// </summary>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <typeparam name="TConcrete">Object which will be constructed and returned.</typeparam>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        public void RegisterType<TService, TConcrete>(Lifetime lifetime = Lifetime.Scoped) 
            where TService : class
            where TConcrete : TService
        {
            RegisterType(typeof (TService), typeof (TConcrete), lifetime);
        }

        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="concrete">Type to create, will be registered as the implemented interfaces.</param>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        public void RegisterConcrete(Type concrete, Lifetime lifetime = Lifetime.Scoped)
        {
            var registration = CreateRegistration(concrete, lifetime);
            registration.AddServices(_serviceFilter);
            Add(registration);
        }


        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="service">Services which is requested from the container.</param>
        /// <param name="factory">Delegate used to produce the instance.</param>
        /// <param name="lifetime">Lifetime of the object that implements the service.</param>
        public void RegisterService(Type service, Func<IServiceLocator, object> factory,
                                 Lifetime lifetime = Lifetime.Scoped)
        {
            var registration = CreateRegistration(null, lifetime);
            registration.AddService(service);
            registration.InstanceStrategy = new DelegateStrategy(factory, lifetime);
            Add(registration);
        }

        /// <summary>
        /// Register a type
        /// </summary>
        /// <param name="service">Type which will be requested</param>
        /// <param name="concrete">Class which will be constructed and returned.</param>
        /// <param name="lifetime">Lifetime of the object that implements the service</param>
        public void RegisterType(Type service, Type concrete, Lifetime lifetime = Lifetime.Scoped)
        {
            if (!service.IsAssignableFromGeneric(concrete) && !service.IsAssignableFrom(concrete))
                throw new InvalidOperationException(string.Format("Type {0} is not assignable from {1}.",
                                                                  service.FullName, concrete.FullName));

            var registration = CreateRegistration(concrete, lifetime);
            registration.AddService(service);
            Add(registration);
        }

        /// <summary>
        /// Register an singleton
        /// </summary>
        /// <typeparam name="TService">Type which will be requested</typeparam>
        /// <param name="instance">Object which will be returned</param>
        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            var registration = CreateRegistration(null, Lifetime.Singleton);
            registration.AddService(typeof (TService));
            registration.InstanceStrategy = new ExistingInstanceStrategy(instance);
            Add(registration);
        }

        /// <summary>
        /// Register an singleton
        /// </summary>
        /// <param name="service">Type which will be requested</param>
        /// <param name="instance">Object which will be returned</param>
        public void RegisterInstance(Type service, object instance)
        {
            var registration = CreateRegistration(null, Lifetime.Singleton);
            registration.AddService(service);
            registration.InstanceStrategy = new ExistingInstanceStrategy(instance);
            Add(registration);
        }

        #endregion

        /// <summary>
        /// Register a component
        /// </summary>
        /// <param name="concreteType">Class to create</param>
        /// <param name="lifetime">Lifetime to use</param>
        protected virtual void RegisterComponent(Type concreteType, Lifetime lifetime)
        {
            var registration = CreateRegistration(concreteType, lifetime);
            registration.AddServices(_serviceFilter);
            Add(registration);
        }

        /// <summary>
        /// Add another registration.
        /// </summary>
        /// <param name="registration">Registration to add</param>
        protected virtual void Add(ComponentRegistration registration)
        {
            if (registration == null) throw new ArgumentNullException("registration");
            _registrations.Add(registration);
        }

        /// <summary>
        /// Factory method for the component registration
        /// </summary>
        /// <param name="concrete">concrete class</param>
        /// <param name="lifetime">lifetime</param>
        /// <returns>Registration object.</returns>
        protected virtual ComponentRegistration CreateRegistration(Type concrete, Lifetime lifetime)
        {
            return new ComponentRegistration(concrete, lifetime);
        }
    }
}