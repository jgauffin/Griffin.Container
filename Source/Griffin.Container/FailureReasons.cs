using System;
using System.Collections.Generic;
using System.Linq;

namespace Griffin.Container
{
    /// <summary>
    /// Failed to resolve a type
    /// </summary>
    public class FailureReasons
    {
        private readonly List<ConstructorFailedReason> _reasons = new List<ConstructorFailedReason>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FailureReasons"/> class.
        /// </summary>
        /// <param name="type">The type which cannot be resolved properly.</param>
        public FailureReasons(Type type)
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

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var temp = "";
            foreach (var reason in _reasons)
            {
                temp += string.Format("Failed to use constructor '{0}({1})' due to missing service '{2}'\r\n", Type.Name,
                                      string.Join(", ", reason.Constructor.GetParameters().Select(x => string.Format("{0} {1}", x.ParameterType.Name, x.Name))),
                                      reason.MissingService.Name);
            }

            return temp;
        }
    }
}