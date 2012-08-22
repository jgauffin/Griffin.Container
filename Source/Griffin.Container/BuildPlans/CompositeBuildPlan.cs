using System;
using System.Collections.Generic;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// Used for constructor parameters that is of type <c><![CDATA[IEnumerable<T>]]></c>.
    /// </summary>
    /// <remarks>Contains two or more build plans which is used for the construction</remarks>
    public class CompositeBuildPlan : IBuildPlan
    {
        private readonly IBuildPlan[] _buildPlans;
        private readonly Type _serviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="CompositeBuildPlan"/> class.
        /// </summary>
        /// <param name="serviceType">Should be type of <c><![CDATA[IEnumerable<TheType>]]></c></param>
        /// <param name="buildPlans">The build plans.</param>
        public CompositeBuildPlan(Type serviceType, IBuildPlan[] buildPlans)
        {
            _serviceType = serviceType;
            _buildPlans = buildPlans;
        }

        #region IBuildPlan Members

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// If an existing or an new instance is returned.
        /// </returns>
        /// <remarks>
        /// Use one of the set methods to assigned the instance.
        /// </remarks>
        public InstanceResult GetInstance(CreateContext context, out object instance)
        {
            var array = (object[]) Array.CreateInstance(_serviceType, _buildPlans.Length);
            var index = 0;
            foreach (var buildPlan in _buildPlans)
            {
                object innerInstance;
                buildPlan.GetInstance(context, out innerInstance);

                array[index++] = innerInstance;
            }

            instance = array;
            return InstanceResult.Loaded;
        }

        /// <summary>
        /// gets services that the concrete implements.
        /// </summary>
        public Type[] Services
        {
            get { return new[] {_serviceType}; }
        }

        /// <summary>
        /// Gets lifetime of the object.
        /// </summary>
        public Lifetime Lifetime
        {
            get { return Lifetime; }
        }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        public string DisplayName
        {
            get
            {
                var value = _serviceType.FullName.Substring(0, _serviceType.FullName.IndexOf('`')) + "<";
                var genericArgs = _serviceType.GetGenericArguments();
                var list = new List<string>();
                for (var i = 0; i < genericArgs.Length; i++)
                {
                    value += "{" + i + "},";
                    list.Add(genericArgs[i].ToString());
                }
                value = value.TrimEnd(',');
                value += ">";
                value = string.Format(value, list);
                return value;
            }
        }

        /// <summary>
        /// Callback invoked each time a new instance is created.
        /// </summary>
        /// <param name="callback">Callback to invoke</param>
        public void SetCreateCallback(ICreateCallback callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            foreach (var buildPlan in _buildPlans)
            {
                buildPlan.SetCreateCallback(callback);
            }
        }

        #endregion
    }
}