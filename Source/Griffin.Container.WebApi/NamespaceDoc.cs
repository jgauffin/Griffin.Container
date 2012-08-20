using System.Runtime.CompilerServices;

namespace Griffin.Container.WebApi
{
    /// <summary>
    /// WebApi integration package for Griffin.Container.
    /// </summary>
    /// <remarks>
    /// Install it using nuget:
    /// 
    /// <code>install-package griffin.container.webapi</code>
    /// </remarks>
    /// <example>
    /// <code>
    /// var registrar = new ContainerRegistrar();
    /// 
    /// // Sample registration of business classes
    /// registrar.RegisterModules(typeof(SomeRepositoryClass).Assembly);
    /// 
    /// // the api controllers
    /// registrar.RegisterApiControllers(typeof(WebApiApplication).Assembly);
    ///
    /// // build the container
    /// _container = registrar.Build();
    /// 
    /// // Configure Dependency Injection in WebApi
    /// GlobalConfiguration.Configuration.DependencyResolver = new GriffinWebApiDependencyResolver(_container);
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }
}