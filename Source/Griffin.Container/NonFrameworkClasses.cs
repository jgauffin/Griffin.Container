using System;

namespace Griffin.Container
{
    /// <summary>
    /// Filter used when the registrar is looking for the services that a class implement.
    /// </summary>
    /// <remarks>This filter ignores all .NET Framework classes.</remarks>
    public class NonFrameworkClasses : IServiceFilter
    {
        #region IServiceFilter Members

        /// <summary>
        /// Determines if a concrete can be registered as the specified type.
        /// </summary>
        /// <param name="service">Implemented service</param>
        /// <returns>
        /// true if the class should be registered as the specified service; otherwise false.
        /// </returns>
        public bool CanRegisterAs(Type service)
        {
            if (service == null) throw new ArgumentNullException("service");
            if (string.IsNullOrEmpty(service.Namespace))
                return false;

            if (service.Namespace.StartsWith("System."))
                return false;
            if (service.Namespace.StartsWith("Microsoft."))
                return false;

            return true;
        }

        #endregion
    }
}