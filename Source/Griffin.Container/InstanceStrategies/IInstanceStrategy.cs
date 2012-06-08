namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// How to manage instances
    /// </summary>
    public interface IInstanceStrategy
    {
        /// <summary>
        /// Get instance.
        /// </summary>
        /// <param name="context">Information used to create/fetch instance.</param>
        /// <param name="instance">Instance that was loaded/created</param>
        /// <returns>If the instance was created or loaded from a storage.</returns>
        InstanceResult GetInstance(IInstanceStrategyContext context, out object instance);

        /// <summary>
        /// Gets if the strategy can generate an instance by itself.
        /// </summary>
        /// <remarks>true if the strategy holds an pre-created instance or if it can create an instance without the build plan.</remarks>
        bool IsInstanceFactory { get; }
    }
}