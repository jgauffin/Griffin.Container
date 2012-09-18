using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using Griffin.Container;
using Griffin.Container.Mvc4;

namespace $rootnamespace$.App_Start
{
    /// <summary>
    /// Griffin.Container configuration
    /// </summary>
    /// <remarks>Add <c>GriffinContainerConfig.Configure()</c> to your global.asax.</remarks>
    public class GriffinContainerConfig
    {
        private static Container _container;

        // add custom registrations in here.
        protected static void Register(ContainerRegistrar registrar)
        {
        }

        public static void Configure()
        {
            var registrar = new ContainerRegistrar();
            registrar.RegisterApiControllers(Assembly.GetExecutingAssembly());
            registrar.RegisterControllers(Assembly.GetExecutingAssembly());

            Register(registrar);

            _container = registrar.Build();
            GlobalConfiguration.Configuration.DependencyResolver = new GriffinWebApiDependencyResolver(_container);
            DependencyResolver.SetResolver(new GriffinDependencyResolver(_container));
        }
    }
}