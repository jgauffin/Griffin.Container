using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Used to select which classes we should decorate
    /// </summary>
    public interface IDecoratorFilter
    {
        /// <summary>
        /// Determins if an instance should be decorated or not.
        /// </summary>
        /// <param name="context">Context</param>
        /// <returns><c>true</c> if we should attach a decorator; otherwise <c>false</c>.</returns>
        bool CanDecorate(DecoratorContext context);
    }
}
