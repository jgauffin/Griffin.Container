using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Dispatcher;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Adds the child container extension to the created service.
    /// </summary>
    public class ServiceLocationInstanceContextInitializer : IInstanceContextInitializer
    {
        /// <summary>
        /// Provides the ability to modify the newly created <see cref="T:System.ServiceModel.InstanceContext"/> object.
        /// </summary>
        /// <param name="instanceContext">The system-supplied instance context.</param>
        /// <param name="message">The message that triggered the creation of the instance context.</param>
        public void Initialize(InstanceContext instanceContext, Message message)
        {
            instanceContext.Extensions.Add(new ChildContainerContextExtension());
        }
    }
}
