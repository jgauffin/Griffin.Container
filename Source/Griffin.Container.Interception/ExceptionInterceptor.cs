using System;
using Castle.DynamicProxy;
using Griffin.Container.Interception.Logging;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Class which decorates each created instance.
    /// </summary>
    public class ExceptionInterceptor : IInterceptor
    {
        private readonly IExceptionLogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionInterceptor"/> class.
        /// </summary>
        /// <param name="logger">Interface used for the logging.</param>
        public ExceptionInterceptor(IExceptionLogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Intercepts each method call.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        public void Intercept(IInvocation invocation)
        {
            try
            {
                invocation.Proceed();
            }
            catch (Exception err)
            {
                var ctx = new CallContext
                              {
                                  Instance = invocation.InvocationTarget,
                                  Arguments = invocation.Arguments,
                                  ReturnValue = invocation.ReturnValue
                              };
                _logger.LogException(ctx, err);
                throw;
            }
        }
    }
}