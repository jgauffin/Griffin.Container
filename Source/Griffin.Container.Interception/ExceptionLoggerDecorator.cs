using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Wraps each method call and log any exceptions using <see cref="IExceptionLogger"/>.
    /// </summary>
    /// <remarks>Will per default decorate all services. Do note that the exceptions 
    /// are rethrown after the logging (without affecting the callstack)</remarks>
    /// <example>
    /// <code>
    /// <![CDATA[
    /// // register services
    /// var registrar = new ContainerRegistrar();
    /// registrar.RegisterConcrete<TotalFailure>(Lifetime.Transient);
    /// var container = registrar.Build();
    /// 
    /// // only log transient services
    /// var filter = new DelegateDecoratorFilter(ctx => ctx.Lifetime == Lifetime.Transient);
    /// var decorator = new ExceptionLoggerDecorator(this, filter);
    /// container.AddDecorator(decorator);
    /// 
    /// // exception will be logged.
    /// var tmp = container.Resolve<TotalFailure>();
    /// tmp.Fail("Big!");
    /// ]]>
    /// </code>
    /// </example>
    public class ExceptionLoggerDecorator : CastleDecorator
    {
        private readonly IExceptionLogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLoggerDecorator"/> class.
        /// </summary>
        /// <param name="logger">Used for the actual logging.</param>
        public ExceptionLoggerDecorator(IExceptionLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionLoggerDecorator"/> class.
        /// </summary>
        /// <param name="logger">Used for the actual logging.</param>
        /// <param name="filter">The filter.</param>
        public ExceptionLoggerDecorator(IExceptionLogger logger, IDecoratorFilter filter)
            : base(filter)
        {
            _logger = logger;
        }

        /// <summary>
        /// Allows the decorator to prescan all registered concretes
        /// </summary>
        /// <param name="concretes">All registered concretes</param>
        public override void PreScan(IEnumerable<Type> concretes)
        {

        }

        /// <summary>
        /// Create a new interceptor
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        /// Created interceptor (which will be used to handle the instance)
        /// </returns>
        protected override IInterceptor CreateInterceptor(DecoratorContext context)
        {
            return new ExceptionInterceptor(_logger);
        }
    }
}
