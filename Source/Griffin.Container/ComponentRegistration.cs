using System;
using System.Collections.Generic;
using System.Linq;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container
{
    /// <summary>
    /// Information about a specific component
    /// </summary>
    /// <remarks>Used to create the build plan</remarks>
    public class ComponentRegistration
    {
        private readonly Lifetime _lifetime;
        private readonly List<Type> _services = new List<Type>();


        /// <summary>
        /// Initializes a new instance of the <see cref="ComponentRegistration"/> class.
        /// </summary>
        /// <param name="concreteType">Type to create.</param>
        /// <param name="lifetime">The lifetime.</param>
        public ComponentRegistration(Type concreteType, Lifetime lifetime)
        {
            _lifetime = lifetime;
            ConcreteType = concreteType;
        }

        /// <summary>
        /// Gets type to be created.
        /// </summary>
        public Type ConcreteType { get; private set; }

        /// <summary>
        /// Gets or sets strategy used to handle the instance
        /// </summary>
        public IInstanceStrategy InstanceStrategy { get; set; }

        /// <summary>
        /// Gets services.
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
        /// <param name="service">Service to check for</param>
        /// <returns>true if implementing; otherwise false.</returns>
        public bool Implements(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            return Services.Any(x => x == service);
        }

        /// <summary>
        /// Add a new service.
        /// </summary>
        /// <param name="service">Service that the class implementes.</param>
        public void AddService(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");

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
        }
    }
}