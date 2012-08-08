using System;
using System.Collections.Generic;
using System.Linq;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Contains information about the exception which has been thrown.
    /// </summary>
    public class CommandExceptionInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandExceptionInfo"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="parameters">The parameters.</param>
        /// <param name="exception">The exception.</param>
        public CommandExceptionInfo(string name, IDictionary<string, object> parameters, Exception exception)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (exception == null) throw new ArgumentNullException("exception");

            Name = name;
            Parameters = parameters;
            Exception = exception;
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the parameters.
        /// </summary>
        public IDictionary<string, object> Parameters { get; private set; }

        /// <summary>
        /// Gets the exception.
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var tmp = string.Format("{0}\r\n\tParameters:\r\n", Name);
            foreach (var parameter in Parameters)
            {
                tmp += string.Format("\t\t{0}: {1}\r\n", parameter.Key, parameter.Value);
            }

            tmp += "\tException:\r\n\t\t";
            tmp += Exception.ToString().Replace("\r\n", "\r\n\t\t");

            return tmp;
        }
    }
}