using System;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Add the service location behaviour to a contract (service)
    /// </summary>
    /// <remarks>Adds the context initializer as dispatch behaviour to be able to create/dispose child containers.</remarks>
    public class ServiceLocationContractBehavior : IContractBehavior
    {
        private readonly IInstanceProvider _instanceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceLocationContractBehavior"/> class.
        /// </summary>
        /// <param name="instanceProvider">The instance provider.</param>
        public ServiceLocationContractBehavior(IInstanceProvider instanceProvider)
        {
            if (instanceProvider == null) throw new ArgumentNullException("instanceProvider");
            _instanceProvider = instanceProvider;
        }

        /// <summary>
        /// Configures any binding elements to support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract description to modify.</param>
        /// <param name="endpoint">The endpoint to modify.</param>
        /// <param name="bindingParameters">The objects that binding elements require to support the behavior.</param>
        public void AddBindingParameters(ContractDescription contractDescription, ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description for which the extension is intended.</param>
        /// <param name="endpoint">The endpoint.</param>
        /// <param name="clientRuntime">The client runtime.</param>
        public void ApplyClientBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
        }

        /// <summary>
        /// Implements a modification or extension of the client across a contract.
        /// </summary>
        /// <param name="contractDescription">The contract description to be modified.</param>
        /// <param name="endpoint">The endpoint that exposes the contract.</param>
        /// <param name="dispatchRuntime">The dispatch runtime that controls service execution.</param>
        public void ApplyDispatchBehavior(ContractDescription contractDescription, ServiceEndpoint endpoint, DispatchRuntime dispatchRuntime)
        {
            dispatchRuntime.InstanceProvider = _instanceProvider;
            dispatchRuntime.InstanceContextInitializers.Add(new ServiceLocationInstanceContextInitializer());
        }

        /// <summary>
        /// Implement to confirm that the contract and endpoint can support the contract behavior.
        /// </summary>
        /// <param name="contractDescription">The contract to validate.</param>
        /// <param name="endpoint">The endpoint to validate.</param>
        public void Validate(ContractDescription contractDescription, ServiceEndpoint endpoint)
        {
        }
    }
}