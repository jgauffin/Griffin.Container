namespace Griffin.Container.Commands
{
    /// <summary>
    /// Used by <see cref="ExceptionDecorator{T}"/> for the actual logging
    /// </summary>
    public interface IExceptionLogger
    {
        /// <summary>
        /// Log an exception
        /// </summary>
        /// <param name="exceptionInfo">Information about the exception</param>
        void Log(CommandExceptionInfo exceptionInfo);
    }
}