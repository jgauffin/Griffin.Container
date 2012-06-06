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
        /// <returns>Instance if found; otherwise null.</returns>
        object GetInstance(CreateContext context);

        /// <summary>
        /// Gets lifetime of the object.
        /// </summary>
        Lifetime Lifetime { get;  }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        string DisplayName { get; }

    }
}