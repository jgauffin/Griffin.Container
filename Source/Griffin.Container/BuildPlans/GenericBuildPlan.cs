using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Griffin.Container.InstanceStrategies;

namespace Griffin.Container.BuildPlans
{
    /// <summary>
    /// Used to build generics.
    /// </summary>
    public class GenericBuildPlan : ConcreteBuildPlan
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type to construct.</param>
        /// <param name="lifetime">The lifetime.</param>
        /// <param name="instanceStrategy">Used to either fetch or create an instance.</param>
        public GenericBuildPlan(Type concreteType, Lifetime lifetime, IInstanceStrategy instanceStrategy) : base(concreteType, lifetime, instanceStrategy)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericBuildPlan"/> class.
        /// </summary>
        /// <param name="concreteType">Type of the concrete.</param>
        /// <param name="instanceStrategy">The instance strategy.</param>
        public GenericBuildPlan(Type concreteType, IInstanceStrategy instanceStrategy) : base(concreteType, instanceStrategy)
        {
        }

        protected override ObjectActivator GetCreateDelegate()
        {
            return null;
        }

        /// <summary>
        /// Creates the actual instance
        /// </summary>
        /// <param name="context"></param>
        /// <param name="arguments">Constructor arguments</param>
        /// <returns>
        /// Created instance.
        /// </returns>
        /// <remarks>Uses the </remarks>
        protected override object Create(CreateContext context, object[] arguments)
        {
            var type = ConcreteType.MakeGenericType(context.RequestedService.GetGenericArguments());
            return Activator.CreateInstance(type, arguments);
        }
    }
}
