using System;
using System.ServiceModel;
using System.ServiceModel.Description;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Service host which uses an inversion of control container.
    /// </summary>
    /// <remarks>Resolved all contract and service behaviours when building the service. So register any custom behaviours</remarks>
    public class ServiceLocatorServiceHost : ServiceHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocatorServiceHost"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <param name="serviceConcrete">Type of the service.</param>
        /// <param name="baseAddresses">The base addresses.</param>
        public ServiceLocatorServiceHost(IParentContainer container, Type serviceConcrete, params Uri[] baseAddresses)
            : base(serviceConcrete, baseAddresses)
        {
            if (container == null)
            {
                throw new ArgumentNullException("container");
            }

            ApplyServiceBehaviors(container);
            ApplyContractBehaviors(container);

            foreach (var contractDescription in ImplementedContracts.Values)
            {
                var contractBehavior =
                    new ServiceLocationContractBehavior(new ServiceLocatorInstanceProvider(container,
                                                                                           contractDescription.
                                                                                               ContractType));

                contractDescription.Behaviors.Add(contractBehavior);
            }
        }

        /// <summary>
        /// Applies the contract behaviors.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        private void ApplyContractBehaviors(IServiceLocator serviceLocator)
        {
            var registeredContractBehaviors = serviceLocator.ResolveAll<IContractBehavior>();
            foreach (var contractBehavior in registeredContractBehaviors)
            {
                foreach (var contractDescription in ImplementedContracts.Values)
                {
                    contractDescription.Behaviors.Add(contractBehavior);
                }
            }
        }

        /// <summary>
        /// Applies the service behaviors.
        /// </summary>
        /// <param name="serviceLocator">The service locator.</param>
        private void ApplyServiceBehaviors(IServiceLocator serviceLocator)
        {
            var registeredServiceBehaviors = serviceLocator.ResolveAll<IServiceBehavior>();
            foreach (var serviceBehavior in registeredServiceBehaviors)
            {
                Description.Behaviors.Add(serviceBehavior);
            }
        }
    }
}