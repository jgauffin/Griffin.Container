using System;
using System.Web;
using Griffin.Container.Mvc5;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;

[assembly: PreApplicationStartMethod(typeof (ContainerModule), "Register")]

namespace Griffin.Container.Mvc5
{
    /// <summary>
    /// disposes child container in <see cref="GriffinDependencyResolver"/> when request ends.
    /// </summary>
    public class ContainerModule : IHttpModule
    {
        private static bool _registered = false;

        #region IHttpModule Members

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"/> that provides access to the methods, properties, and events common to all application objects within an ASP.NET application </param>
        public void Init(HttpApplication context)
        {
            context.BeginRequest += OnBeginRequest;
            context.EndRequest += (sender, args) => GriffinDependencyResolver.DisposeChildContainer();
        }

        private void OnBeginRequest(object sender, EventArgs e)
        {
            
        }

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module that implements <see cref="T:System.Web.IHttpModule"/>.
        /// </summary>
        public void Dispose()
        {
        }

        #endregion

        /// <summary>
        /// Register the module in the pipeline
        /// </summary>
        public static void Register()
        {
            if (_registered)
                return;
            _registered = true;
            DynamicModuleUtility.RegisterModule(typeof (ContainerModule));
        }
    }
}