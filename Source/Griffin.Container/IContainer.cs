using System;
using System.Linq;
using System.Text;

namespace Griffin.Container
{
    public interface IParentContainer : IServiceLocator
    {
        IChildContainer CreateChildContainer();
    }

    public interface IChildContainer : IServiceLocator, IDisposable
    {
        
    }

    public class ComponentAttribute : Attribute
    {
        public Lifetime Lifetime { get; set; }
    }

    public interface IContainerModule
    {
        void Register(IContainerRegistrar registrar);
    }

}
