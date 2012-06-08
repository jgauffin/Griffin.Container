using System;

namespace Griffin.Container.Interception
{
    /// <summary>
    /// Invoked by <see cref="ExceptionLoggerDecorator"/>
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// Called when an exception have been raised.
        /// </summary>
        /// <param name="context">context</param>
        /// <param name="err">thrown exception</param>
        void LogException(CallContext context, Exception err);
    }
}