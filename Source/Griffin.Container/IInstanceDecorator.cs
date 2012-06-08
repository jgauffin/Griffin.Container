using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container
{
    /// <summary>
    /// Decorator pattern. Wraps instances which have been created and being returned.
    /// </summary>
    public interface IInstanceDecorator
    {
        /// <summary>
        /// Allows the decorator to prescan all registered concretes
        /// </summary>
        /// <param name="concretes">All registered concretes</param>
        /// <remarks>Makes it possible for the decorator to map up all concretes that it would like to decorate.</remarks>
        void PreScan(IEnumerable<Type> concretes);

        /// <summary>
        /// Decorate the specified instance.
        /// </summary>
        /// <param name="context"></param>
        void Decorate(DecoratorContext context);
    }
}
