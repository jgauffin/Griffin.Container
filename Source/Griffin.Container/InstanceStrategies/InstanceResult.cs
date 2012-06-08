namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// How the strategy got the instance.
    /// </summary>
    public enum InstanceResult
    {
        /// <summary>
        /// Created a new object
        /// </summary>
        Created,

        /// <summary>
        /// Loaded from storage
        /// </summary>
        Loaded
    }
}