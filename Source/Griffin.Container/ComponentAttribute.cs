using System;

namespace Griffin.Container
{
    /// <summary>
    /// Decorate your classes with this attribute to get automatic configuraiton
    /// </summary>
    /// <remarks>Use <see cref="IContainerRegistrar.RegisterComponents(Lifetime, string, string)"/> to register all classes which have been decorated with this attribute. The 
    /// classes will be registered as all implementend services (except those specified in <see cref="IServiceFilter"/>)</remarks>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets lifetime
        /// </summary>
        public Lifetime Lifetime { get; set; }

        /// <summary>
        /// Register class as itself.
        /// </summary>
        public bool RegisterAsSelf { get; set; }

    }
}