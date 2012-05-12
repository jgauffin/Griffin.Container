using System;
using System.Linq;
using System.Text;

namespace Griffin.Container
{
    /// <summary>
    /// Child container, holds all scoped services.
    /// </summary>
    /// <remarks>All scoped services are disposed when the child container is disposed.</remarks>
    public interface IChildContainer : IServiceLocator, IDisposable
    {
        
    }
}
