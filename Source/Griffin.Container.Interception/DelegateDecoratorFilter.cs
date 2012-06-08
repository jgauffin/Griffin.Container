using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Uses a delegate to determine if the intances can be decorated.
    /// </summary>
    public class DelegateDecoratorFilter : IDecoratorFilter
    {
        private readonly Func<DecoratorContext, bool> _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelegateDecoratorFilter"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public DelegateDecoratorFilter(Func<DecoratorContext, bool> filter)
        {
            if (filter == null) throw new ArgumentNullException("filter");
            _filter = filter;
        }

        /// <summary>
        /// Determins if an instance should be decorated or not.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns>
        ///   <c>true</c> if we should attach a decorator; otherwise <c>false</c>.
        /// </returns>
        public bool CanDecorate(DecoratorContext context)
        {
            return _filter(context);
        }
    }
}
