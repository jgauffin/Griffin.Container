using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// Used for constructors that want a list of services when no services has been registered.
    /// </summary>
    public class EmptyListBuildPlan : IBuildPlan
    {
        private readonly Type _service;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmptyListBuildPlan"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        public EmptyListBuildPlan(Type service)
        {
            _service = service;
        }

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// If an existing or an new instance is returned.
        /// </returns>
        /// <remarks>
        /// Use one of the set methods to assigned the instance.
        /// </remarks>
        public InstanceResult GetInstance(CreateContext context, out object instance)
        {
            instance = Array.CreateInstance(_service, 0);
            return InstanceResult.Loaded;
        }

        /// <summary>
        /// Gets services that the concrete implements.
        /// </summary>
        public Type[] Services { get { return new Type[] { _service }; } }

        /// <summary>
        /// Gets lifetime of the object.
        /// </summary>
        public Lifetime Lifetime { get { return Lifetime.Transient; } }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        public string DisplayName { get { return _service.Name + "[]"; } }

        /// <summary>
        /// Callback invoked each time a new instance is created.
        /// </summary>
        /// <param name="callback">Callback to invoke</param>
        public void SetCreateCallback(ICreateCallback callback)
        {
            
        }
    }
}
