namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Context used when determin if a new instance should be created or not.
    /// </summary>
    public interface IInstanceStrategyContext
    {
        /// <summary>
        /// Gets build plan
        /// </summary>
        BuildPlan BuildPlan { get; }

        /// <summary>
        /// Storage for singletons.
        /// </summary>
        /// <remarks>Use the build plan as key</remarks>
        IInstanceStorage SingletonStorage { get; }

        /// <summary>
        /// Storage for scoped objects
        /// </summary>
        /// /// <remarks>Use the build plan as key</remarks>
        IInstanceStorage ScopedStorage { get; }


        /// <summary>
        /// Gets container which is requesting an instance.
        /// </summary>
        IServiceLocator Container { get; }

        /// <summary>
        /// Create a new isntance
        /// </summary>
        /// <returns>Created instanc.e</returns>
        object CreateInstance();
    }
}