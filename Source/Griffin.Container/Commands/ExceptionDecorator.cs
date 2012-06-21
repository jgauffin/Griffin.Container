using System;
using System.Collections.Generic;
using System.Reflection;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Decorates all commands to allow you to log all exceptions.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ExceptionDecorator<T> : IHandlerOf<T> where T : class, ICommand
    {
        private readonly IHandlerOf<T> _inner;
        private readonly IExceptionLogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionDecorator{T}"/> class.
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="inner">The inner.</param>
        public ExceptionDecorator(IExceptionLogger logger, IHandlerOf<T> inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            _logger = logger;
            _inner = inner;
        }

        #region IHandlerOf<T> Members

        /// <summary>
        /// Invokes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Invoke(T command)
        {
            try
            {
                _inner.Invoke(command);
            }
            catch (Exception err)
            {
                var parameters = new Dictionary<string, object>();
                foreach (PropertyInfo propertyInfo in command.GetType().GetProperties())
                {
                    object value = propertyInfo.GetValue(command, null);
                    parameters.Add(propertyInfo.Name, value);
                }

                var info = new CommandExceptionInfo(command.GetType().Name, parameters, err);
                _logger.Log(info);
                throw;
            }
        }

        #endregion
    }
}