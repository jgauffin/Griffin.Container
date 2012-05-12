using System;
using System.Collections.Generic;

namespace Griffin.Container
{
    /// <summary>
    /// Failed to resolve a type
    /// </summary>
    public class TypeResolutionFailed
    {
        private readonly List<ConstructorFailedReason> _reasons = new List<ConstructorFailedReason>();

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeResolutionFailed"/> class.
        /// </summary>
        /// <param name="type">The type which cannot be resolved properly.</param>
        public TypeResolutionFailed(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");
            Type = type;
        }

        /// <summary>
        /// Get type which could not be constructed.
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// A constructor which failed.
        /// </summary>
        /// <param name="reason">Why the constructor failed.</param>
        public void Add(ConstructorFailedReason reason)
        {
            _reasons.Add(reason);
        }
    }
}