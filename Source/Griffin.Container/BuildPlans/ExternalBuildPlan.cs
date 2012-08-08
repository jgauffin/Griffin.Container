using System;
using System.Collections.Generic;
using System.Linq;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// Maps a service to a build plan which is generated outside.
    /// </summary>
    public class ExternalBuildPlan : IBuildPlan
    {
        private readonly Lifetime _lifetime;
        private readonly IInstanceStrategy _instanceStrategy;
        private ICreateCallback _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalBuildPlan"/> class.
        /// </summary>
        /// <param name="services">The services that the concrete implements..</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="instanceStrategy">The instance strategy.</param>
        public ExternalBuildPlan(IEnumerable<Type> services, Lifetime lifetime, IInstanceStrategy instanceStrategy)
        {
            if (instanceStrategy == null) throw new ArgumentNullException("instanceStrategy");
            _lifetime = lifetime;
            _instanceStrategy = instanceStrategy;
            Services = services.ToArray();
        }

        /// <summary>
        /// Get the instance.
        /// </summary>
        /// <param name="context">Context used to create instances.</param>
        /// <param name="instance">The instance.</param>
        /// <returns>
        /// Instance if found; otherwise null.
        /// </returns>
        public InstanceResult GetInstance(CreateContext context, out object instance)
        {
            var strategyContext = new ExternalInstanceStrategyContext
            {
                BuildPlan = this,
                CreateContext = context
            };

            var result =  _instanceStrategy.GetInstance(strategyContext, out instance);
            if (result == InstanceResult.Created && _callback != null)
                instance = _callback.InstanceCreated(context, this, instance);

            return result;
        }

        /// <summary>
        /// Gets services that the concrete implements.
        /// </summary>
        public Type[] Services { get; private set; }

        /// <summary>
        /// Gets lifetime of the object.
        /// </summary>
        public Lifetime Lifetime
        {
            get { return _lifetime; }
        }

        /// <summary>
        /// Either name of the concrete or anything else which can help the user to identify the registration.
        /// </summary>
        public string DisplayName
        {
            get { return _instanceStrategy.GetType().FullName; }
        }

        /// <summary>
        /// Callback invoked each time a new instance is created.
        /// </summary>
        /// <param name="callback">Callback to invoke</param>
        public void SetCreateCallback(ICreateCallback callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");
            _callback = callback;
        }

        class ExternalInstanceStrategyContext : IInstanceStrategyContext
        {
            public IBuildPlan BuildPlan { get;  set; }
            public CreateContext CreateContext { get; set; }
        }
    }
}