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
        /// Decorate the specified instance.
        /// </summary>
        /// <param name="context"></param>
        void Decorate(DecoratorContext context);
    }
}
