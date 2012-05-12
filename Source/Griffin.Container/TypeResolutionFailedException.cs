using System;

namespace Griffin.Container
{
    /// <summary>
    /// Thrown when a dependency can not be resolved.
    /// </summary>
    public class TypeResolutionFailedException : Exception
    {
        private readonly TypeResolutionFailed _error;

        public TypeResolutionFailedException(TypeResolutionFailed error)
            : base("Failed to resolve " + error.Type)
        {
            _error = error;
        }
    }
}