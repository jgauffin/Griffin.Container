using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;

namespace Griffin.Container.WebApi
{
    /// <summary>
    /// Extension methods making it easier to 
    /// </summary>
    public static class GriffinContainerExtensions
    {
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
                    x => typeof (ApiController).IsAssignableFrom(x));

            foreach (var controller in controllers)
            {
                registrar.RegisterConcrete(controller, Lifetime.Scoped);
            }
        }
    }
}