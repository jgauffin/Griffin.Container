using System;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Produces <see cref="ServiceLocatorServiceHost"/> hosts.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You can assign an action to <see cref="ConfigurationCallback"/> to make any custom service host configurations.</para>
    /// <para>The service host factory honors the <see cref="ServiceBehaviorAttribute.InstanceContextMode"/> property. Use it to create a singleton/scoped/transient. Default setting is scoped</para>
    /// </remarks>
    public class ServiceHostFactory : System.ServiceModel.Activation.ServiceHostFactory
    {
        private static IParentContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceHostFactory"/> class.
        /// </summary>
        public ServiceHostFactory()
        {

        }

        /// <summary>
        /// Invoked when the service host is configured.
        /// </summary>
        /// <remarks>Assign a callback if you want to add any custom service host configuration each time a new host is created.</remarks>
        public static Action<ServiceHostBase> ConfigurationCallback { get; set; }

        /// <summary>
        /// Creates a <see cref="T:System.ServiceModel.ServiceHost"/> for a specified type of service with a specific base address.
        /// </summary>
        /// <param name="serviceType">Specifies the type of service to host.</param>
        /// <param name="baseAddresses">The <see cref="T:System.Array"/> of type <see cref="T:System.Uri"/> that contains the base addresses for the service hosted.</param>
        /// <returns>
        /// A <see cref="T:System.ServiceModel.ServiceHost"/> for the type of service specified with a specific base address.
        /// </returns>
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            if (serviceType == null) throw new ArgumentNullException("serviceType");

            ServiceBehaviorAttribute attribute;
            var lifetime = GetLifetime(serviceType, out attribute);

            if (_container == null)
            {
                var registrar = new ContainerRegistrar();
                registrar.RegisterConcrete(serviceType, lifetime);
                registrar.RegisterModules(serviceType.Assembly);
                _container = registrar.Build();
            }

            var host = new ServiceLocatorServiceHost(_container, serviceType, baseAddresses);
            if (ConfigurationCallback != null)
                ConfigurationCallback(host);

            return host;
        }

        private static Lifetime GetLifetime(Type serviceType, out ServiceBehaviorAttribute attribute)
        {
            attribute =
                serviceType.GetCustomAttributes(typeof (ServiceBehaviorAttribute), true).Cast<ServiceBehaviorAttribute>()
                    .FirstOrDefault();

            var lifetime = Lifetime.Scoped;
            if (attribute != null)
            {
                switch (attribute.InstanceContextMode)
                {
                    case InstanceContextMode.Single:
                        lifetime = Lifetime.Singleton;
                        break;
                    case InstanceContextMode.PerCall:
                        lifetime = Lifetime.Transient;
                        break;
                }
            }
            return lifetime;
        }
    }
}