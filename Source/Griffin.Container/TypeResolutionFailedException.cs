using System;

namespace Griffin.Container
{
    /// <summary>
    /// Thrown when a dependency can not be resolved.
    /// </summary>
    public class TypeResolutionFailedException : Exception
    {
        private readonly TypeResolutionFailed _error;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionFailedException"/> class.
        /// </summary>
        /// <param name="error">The error.</param>
        public TypeResolutionFailedException(TypeResolutionFailed error)
            : base("Failed to resolve " + error.Type)
        {
            _error = error;
        }

        /// <summary>
        /// Gets reason for the error
        /// </summary>
        public TypeResolutionFailed Error
        {
            get { return _error; }
        }

    }
}