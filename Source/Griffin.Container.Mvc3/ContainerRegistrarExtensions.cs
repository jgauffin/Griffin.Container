using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Griffin.Container.Mvc3
{
    /// <summary>
    /// Extension methods for the HTTP integration
    /// </summary>
    public static class ContainerRegistrarExtensions
    {
        /// <summary>
        /// Register all controllers 
        /// </summary>
        /// <param name="registrar">The registrar</param>
        /// <param name="assembly">Assembly to scan.</param>
        public static void RegisterControllers(this IContainerRegistrar registrar, Assembly assembly)
        {
            var controllerType = typeof (IController);
            foreach (var type in assembly.GetTypes().Where(controllerType.IsAssignableFrom))
            {
                registrar.RegisterConcrete(type, Lifetime.Scoped);
            }
        }
    }
}