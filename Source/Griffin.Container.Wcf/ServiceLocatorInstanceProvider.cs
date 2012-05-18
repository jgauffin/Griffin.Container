using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Create instances using a service locator
    /// </summary>
    /// <remarks>Will also add a child container for scoped services and their sdependencies.</remarks>
    public class ServiceLocatorInstanceProvider : IInstanceProvider
    {
        private readonly IParentContainer _container;
        private readonly Type _contractType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorInstanceProvider"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="contractType"> </param>
        public ServiceLocatorInstanceProvider(IParentContainer container, Type contractType)
        {
            if (container == null) throw new ArgumentNullException("container");
            if (contractType == null) throw new ArgumentNullException("contractType");

            _container = container;
            _contractType = contractType;
        }

        #region IInstanceProvider Members

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <param name="message">The message that triggered the creation of a service object.</param>
        /// <returns>
        /// The service object.
        /// </returns>
        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            var serviceType = instanceContext.Host.Description.ServiceType;
            var childContainer =
                instanceContext.Extensions.Find<ChildContainerContextExtension>().GetChildContainer(_container);

            return childContainer.Resolve(serviceType);
        }

        /// <summary>
        /// Returns a service object given the specified <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The current <see cref="T:System.ServiceModel.InstanceContext"/> object.</param>
        /// <returns>
        /// A user-defined service object.
        /// </returns>
        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        /// <summary>
        /// Called when an <see cref="T:System.ServiceModel.InstanceContext"/> object recycles a service object.
        /// </summary>
        /// <param name="instanceContext">The service's instance context.</param>
        /// <param name="instance">The service object to be recycled.</param>
        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
            instanceContext.Extensions.Find<ChildContainerContextExtension>().DisposeOfChildContainer();
        }

        #endregion
    }
}