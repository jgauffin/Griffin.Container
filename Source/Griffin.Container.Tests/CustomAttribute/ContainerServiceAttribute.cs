using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.Tests.CustomAttribute
{
    /// <summary>
    /// 
    /// </summary>
    class ContainerServiceAttribute : Attribute, IAttributeRegistrar
    {
        /// <summary>
        /// Register the class in the container
        /// </summary>
        /// <param name="concrete">Class which has been decorated with the attribute.</param>
        /// <param name="registrar">The container registrar.</param>
        public void Register(Type concrete, IContainerRegistrar registrar)
        {
            registrar.RegisterConcrete(concrete, Lifetime.Default);
        }
    }
}
