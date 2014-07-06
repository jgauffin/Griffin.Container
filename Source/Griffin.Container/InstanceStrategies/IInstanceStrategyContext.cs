using Griffin.Container.BuildPlans;

namespace Griffin.Container.InstanceStrategies
{
    /// <summary>
    /// Context used when determine if a new instance should be created or not.
    /// </summary>
    public interface IInstanceStrategyContext
    {
        /// <summary>
        /// Gets build plan
        /// </summary>
        IBuildPlan BuildPlan { get; }

        /// <summary>
        /// Gets context specified by the container
        /// </summary>
        /// <remarks>Use one of the set methods to </remarks>
        CreateContext CreateContext { get; }
    }

    /// <summary>
    /// context for instance strategies.
    /// </summary>
    public interface IConcreteInstanceStrategyContext : IInstanceStrategyContext
    {
        /// <summary>
        /// Create instance using the build plans factory method.
        /// </summary>
        object CreateInstance();
    }

    /*
     * 
        /// <summary>
        /// Create a new isntance
        /// </summary>
        /// <returns>Created instanc.e</returns>
        object CreateInstance();*/
}