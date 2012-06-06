using System;

namespace Griffin.Container
{
    /// <summary>
    /// Thrown when a dependency is missing
    /// </summary>
    public class DependencyNotRegisteredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DependencyNotRegisteredException"/> class.
        /// </summary>
        /// <param name="concreteType">Type of the concrete.</param>
        /// <param name="missingService">The missing service.</param>
        public DependencyNotRegisteredException(Type concreteType, Type missingService)
            : base(string.Format("Failed to resolve '{0}' which is required by '{1}'.", concreteType, missingService))
        {
            ConcreteType = concreteType;
            MissingService = missingService;
        }

        /// <summary>
        /// Gets type being built
        /// </summary>
        public Type ConcreteType { get; private set; }

        /// <summary>
        /// Gets service that <see cref="ConcreteType"/> want's (but is not registered in the container)
        /// </summary>
        public Type MissingService { get; private set; }
    }
}