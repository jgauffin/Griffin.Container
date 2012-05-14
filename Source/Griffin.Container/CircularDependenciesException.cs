using System;
using System.Collections.Generic;
using System.Linq;

namespace Griffin.Container
{
    /// <summary>
    /// Circular depenencies.
    /// </summary>
    public class CircularDependenciesException : Exception
    {
        private readonly IEnumerable<ConcreteBuildPlan> _plans;

        /// <summary>
        /// Initializes a new instance of the <see cref="CircularDependenciesException"/> class.
        /// </summary>
        /// <param name="errMsg">The err MSG.</param>
        /// <param name="plans">The plans.</param>
        public CircularDependenciesException(string errMsg, IEnumerable<ConcreteBuildPlan> plans)
            : base(errMsg)
        {
            if (plans == null) throw new ArgumentNullException("plans");
            _plans = plans;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                var plans = _plans.Aggregate("", (current, plan) => current + (plan.ConcreteType.FullName + "  => "));
                plans = plans.Remove(plans.Length - 4, 4);
                return base.Message + " Path: " + plans;
            }
        }
    }
}