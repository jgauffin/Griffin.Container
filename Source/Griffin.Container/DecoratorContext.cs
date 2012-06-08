using System;
using System.Collections.Generic;
using System.Linq;

namespace Griffin.Container
{
    /// <summary>
    /// Context for <see cref="IInstanceDecorator"/>
    /// </summary>
    public class DecoratorContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DecoratorContext"/> class.
        /// </summary>
        /// <param name="services">The requested service.</param>
        /// <param name="lifetime">The lifetime of the service.</param>
        public DecoratorContext(IEnumerable<Type>  services, Lifetime lifetime)
        {
            Services = services.ToArray();
            Lifetime = lifetime;
        }

        /// <summary>
        /// Gets type of lifetime for the object.
        /// </summary>
        public Lifetime Lifetime { get; private set; }

        /// <summary>
        /// Gets service that the concrete implements
        /// </summary>
        public Type[] Services { get; private set; }

        /// <summary>
        /// Gets service which was requested.
        /// </summary>
        public Type RequestedService { get; set; }

        /// <summary>
        /// Gets or sets current instance.
        /// </summary>
        /// <remarks>Each decorator should swap it with it's own implementation</remarks>
        public object Instance { get; set; }
    }
}