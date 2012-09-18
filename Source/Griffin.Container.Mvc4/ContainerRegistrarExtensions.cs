using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;

namespace Griffin.Container.Mvc4
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
                // no public constructors. A base class?
                if (type.GetConstructors().Length == 0 || type.IsAbstract)
                    continue;

                registrar.RegisterType(type, type, Lifetime.Transient);
            }
        }

        /// <summary>
        /// Scan specified assembly after WebApi controllers.
        /// </summary>
        /// <param name="registrar">Container registrar to register the controllers in.</param>
        /// <param name="assembly">Assembly to scan</param>
        /// <example>
        /// <code>
        /// registrar.RegisterApiControllers(typeof(WebApiApplication).Assembly);
        /// </code>
        /// </example>
        public static void RegisterApiControllers(this IContainerRegistrar registrar, Assembly assembly)
        {
            if (assembly == null) throw new ArgumentNullException("assembly");
            var controllers =
                assembly.GetTypes().Where(
                    x => typeof(ApiController).IsAssignableFrom(x));

            foreach (var controller in controllers)
            {
                // no public constructors. A base class?
                if (controller.GetConstructors().Length == 0 || controller.IsAbstract)
                    continue;

                registrar.RegisterConcrete(controller, Lifetime.Transient);
            }
        }
    }
}