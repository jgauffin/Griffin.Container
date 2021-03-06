﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Information about a specific concrete (which implements one or more services).
    /// </summary>
    /// <remarks>Used to create the build plan</remarks>
    public class ComponentRegistration
    {
        private readonly Lifetime _lifetime;
        private readonly List<Type> _services = new List<Type>();


        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistration"/> class.
        /// </summary>
        /// <param name="concreteType">Type to create, , NULL for services that use factory methods</param>
        /// <param name="lifetime">The lifetime.</param>
        public ComponentRegistration(Type concreteType, Lifetime lifetime)
        {
            if (!Enum.IsDefined(typeof(Lifetime), lifetime))
                throw new InvalidEnumArgumentException(nameof(lifetime), (int)lifetime, typeof(Lifetime));
            _lifetime = lifetime;
            ConcreteType = concreteType;
        }

        /// <summary>
        /// Gets type to be created (NULL for requestedService registrations that use factory methods)
        /// </summary>
        public Type ConcreteType { get; private set; }

        /// <summary>
        /// Gets or sets strategy used to handle the instance (build plan)
        /// </summary>
        public IInstanceStrategy InstanceStrategy { get; set; }

        /// <summary>
        /// Gets services that the class implements
        /// </summary>
        public IEnumerable<Type> Services
        {
            get { return _services; }
        }

        /// <summary>
        /// Gets instance lifetime
        /// </summary>
        public Lifetime Lifetime
        {
            get { return _lifetime; }
        }

        /// <summary>
        /// Checks if the current concrete implements the specified class.
        /// </summary>
        /// <param name="requestedService">Service to check for</param>
        /// <returns>true if implementing; otherwise false.</returns>
        public bool Implements(Type requestedService)
        {
            if (requestedService == null) throw new ArgumentNullException("requestedService");

            if (!requestedService.IsGenericType)
                return Services.Any(x => x == requestedService);

            foreach (var registeredService in Services)
            {
                if (!registeredService.IsGenericType)
                    continue;
                if (registeredService.GetGenericTypeDefinition() != requestedService.GetGenericTypeDefinition())
                    continue;

                var registeredServiceArgs = registeredService.GetGenericArguments();
                var requestedServiceArgs = requestedService.GetGenericArguments();
                if (registeredServiceArgs.Length != requestedServiceArgs.Length)
                    continue;

                if (registeredService.IsGenericTypeDefinition)
                    return true;

                bool found = true;
                for (int i = 0; i < requestedServiceArgs.Length; i++)
                {
                    if (registeredServiceArgs[i] != requestedServiceArgs[i])
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Add a new requestedService.
        /// </summary>
        /// <param name="service">Service that the class implementes.</param>
        public void AddService(Type service)
        {
            if (service == null) throw new ArgumentNullException("requestedService");

            if (!service.IsAssignableFrom(service))
                throw new InvalidOperationException(string.Format("Type {0} do not inherit/implement {1}",
                                                                  ConcreteType.FullName, service));

            _services.Add(service);
        }

        /// <summary>
        /// Add all services which the concrete type implements
        /// </summary>
        /// <param name="serviceFilter">Used to filter implemented services.</param>
        public void AddServices(IServiceFilter serviceFilter)
        {
            if (serviceFilter == null) throw new ArgumentNullException("serviceFilter");

            foreach (var service in ConcreteType.GetInterfaces())
            {
                if (!serviceFilter.CanRegisterAs(service))
                    continue;

                AddService(service);
            }

            if (!Services.Any())
                AddService(ConcreteType);

            // Allow non standard filters do decide whether we should get registered
            // as concrete
            else if (!(serviceFilter is NonFrameworkClasses) && serviceFilter.CanRegisterAs(ConcreteType))
                AddService(ConcreteType);
        }
    }
}