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
        /// <returns>Created/Existing instance.</returns>
        object GetInstance(IInstanceStrategyContext context);
    }
}