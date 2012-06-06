using System;

namespace Griffin.Container
{
    /// <summary>
    /// Thrown when a dependency can not be resolved.
    /// </summary>
    public class TypeResolutionFailedException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionFailedException"/> class.
        /// </summary>
        /// <param name="concreteType">Type of the concrete which could not be built.</param>
        /// <param name="error">The error.</param>
        public TypeResolutionFailedException(Type concreteType, FailureReasons error)
            : base(string.Format("Failed to lookup '{0}'.", concreteType.FullName))
        {
            ConcreteBeingBuilt = concreteType;
            Reasons = error;
        }

        /// <summary>
        /// Gets class being built
        /// </summary>
        public Type ConcreteBeingBuilt { get; set; }

        /// <summary>
        /// Gets why we could not build the class.
        /// </summary>
        public FailureReasons Reasons { get; private set; }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get { return base.Message + "\r\n" + Reasons; }
        }
    }
}