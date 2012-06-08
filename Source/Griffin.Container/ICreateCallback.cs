using Griffin.Container.BuildPlans;

namespace Griffin.Container
{
    /// <summary>
    /// Used to be let the container be able to track all created instances
    /// </summary>
    /// <remarks>A requirement to be able to use decorators</remarks>
    public interface ICreateCallback
    {
        /// <summary>
        /// A new instance have been created
        /// </summary>
        /// <param name="context">Context information</param>
        /// <param name="buildPlan">Build plan which created the instance</param>
        /// <param name="instance">Created instance</param>
        /// <returns>Decorated instance (or the original one)</returns>
        object InstanceCreated(CreateContext context, IBuildPlan buildPlan, object instance);
    }
}