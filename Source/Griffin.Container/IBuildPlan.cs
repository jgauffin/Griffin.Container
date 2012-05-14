namespace Griffin.Container
{
    public interface IBuildPlan
    {
        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <returns>Instance if found; otherwise null.</returns>
        object GetInstance(CreateContext context);
    }
}