using System.Runtime.CompilerServices;

namespace Griffin.Container
{
    /// <summary>
    /// Welcome to the Griffin.Container library.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You need to complete three steps to use the container.
    /// <list type="bullet">
    /// <item>Create a new <see cref="ContainerRegistrar"/> and register all services</item>
    /// <item>Create a new <see cref="ContainerBuilder"/> to build the container</item>
    /// <item>Build the container by invoking <see cref="ContainerBuilder.Build"/></item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <example>
    /// Easy way is to start by decorating all services with the <c>[Component]</c> attribute:
    /// <code>
    /// [Component]
    /// public class MyService : IAmService
    /// {
    /// }
    /// </code>
    /// and then build the container:
    /// <code>
    /// var registrar = new ContainerRegistrar();
    /// registrar.RegisterComponents(Lifetime.Scoped, Assembly.GetExecutingAssembly());
    /// 
    /// var container = registrar.Build();
    /// 
    /// // and then integrate it with your favorite framework..
    /// </code>
    /// </example>
    [CompilerGenerated]
    internal class NamespaceDoc
    {
    }
}