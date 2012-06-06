using System;

namespace Griffin.Container
{
    /// <summary>
    /// Context for <see cref="IInstanceDecorator"/>
    /// </summary>
    public class DecoratorContext
    {
        /// <summary>
        /// Gets service which was requested-
        /// </summary>
        public Type ServiceType { get; private set; }

        /// <summary>
        /// Gets or sets current instance.
        /// </summary>
        /// <remarks>Each decorator should swap it with it's own implementation</remarks>
        public object Instance { get; set; }
    }
}