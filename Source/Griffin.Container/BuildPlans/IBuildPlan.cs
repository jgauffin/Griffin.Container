using System;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// Build plan
    /// </summary>
    /// <remarks>Determines if an instance should be created (and how) or if a previous instance should be returned.</remarks>
    public interface IBuildPlan
    {
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
        InstanceResult GetInstance(CreateContext context, out object instance);

        /// <summary>
        /// gets services that the concrete implements.
        /// </summary>
        Type[] Services { get; }

        /// <summary>
        /// Gets lifetime of the object.
        /// </summary>
        Lifetime Lifetime { get;  }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        string DisplayName { get; }

        /// <summary>
        /// Callback invoked each time a new instance is created.
        /// </summary>
        /// <param name="callback">Callback to invoke</param>
        void SetCreateCallback(ICreateCallback callback);
    }

    
}