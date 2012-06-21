using System;
using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// A failed constructor and the reason.
    /// </summary>
    public class ConstructorFailedReason
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorFailedReason"/> class.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="missingService">Service which could not be located.</param>
        public ConstructorFailedReason(ConstructorInfo constructor, Type missingService)
        {
            if (constructor == null) throw new ArgumentNullException("constructor");
            if (missingService == null) throw new ArgumentNullException("missingService");
            Constructor = constructor;
            MissingService = missingService;
        }

        /// <summary>
        /// Gets tried constructor.
        /// </summary>
        public ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Get service which could not be found
        /// </summary>
        public Type MissingService { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Failed to build type '{0}', could not find service '{1}'.", Constructor.ReflectedType,
                                 MissingService);
        }
    }
}