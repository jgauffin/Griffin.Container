using System;
using System.ServiceModel;

namespace Griffin.Container.Wcf
{
    /// <summary>
    /// Creates/Disposes child container.
    /// </summary>
    public class ChildContainerContextExtension : IExtension<InstanceContext>
    {
        private IChildContainer _childContainer;

        #region IExtension<InstanceContext> Members

        /// <summary>
        /// Attaches the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void Attach(InstanceContext owner)
        {
        }

        /// <summary>
        /// Detaches the specified owner.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public void Detach(InstanceContext owner)
        {
        }

        #endregion

        /// <summary>
        /// Gets the child container.
        /// </summary>
        /// <param name="container">The container.</param>
        /// <returns></returns>
        public IChildContainer GetChildContainer(IParentContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            return _childContainer ?? (_childContainer = container.CreateChildContainer());
        }

        /// <summary>
        /// Disposes the of child container.
        /// </summary>
        public void DisposeOfChildContainer()
        {
            if (_childContainer != null)
                _childContainer.Dispose();
        }
    }
}