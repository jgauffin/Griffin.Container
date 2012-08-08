using System.Web.Http.Dependencies;

namespace Griffin.Container.WebApi
{
    /// <summary>
    /// Extension for the WebApiDependencyResolver.
    /// </summary>
    /// <remarks>Makes it possible to access the current threds <see cref="IDependencyScope"/>.</remarks>
    /// <seealso cref="GriffinWebApiDependencyResolver"/>
    public interface IDependencyScopeTracker
    {
        /// <summary>
        /// Gets the current threads dependency scope
        /// </summary>
        IDependencyScope Current { get; }
    }
}