using System.Web.Http.Dependencies;

namespace Griffin.Container.Mvc5
{
    /// <summary>
    /// Extension for the WebApiDependencyResolver.
    /// </summary>
    /// <remarks>Makes it possible to access the current thread's <see cref="IDependencyScope"/>.</remarks>
    /// <seealso cref="GriffinWebApiDependencyResolver"/>
    public interface IDependencyScopeTracker
    {
        /// <summary>
        /// Gets the current threads dependency scope
        /// </summary>
        IDependencyScope Current { get; }
    }
}