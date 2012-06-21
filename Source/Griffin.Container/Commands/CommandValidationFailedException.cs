using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Thrown by <see cref="ValidationDecorator{T}"/>.
    /// </summary>
    public class CommandValidationFailedException : Exception
    {
        /// <summary>
        /// Gets command which was invoked.
        /// </summary>
        public ICommand Command { get; set; }

        /// <summary>
        /// Gets validation failure reasons
        /// </summary>
        public IEnumerable<ValidationResult> Results { get;private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidationFailedException"/> class.
        /// </summary>
        /// <param name="command">The command that failed.</param>
        /// <param name="results">Validation errors.</param>
        public CommandValidationFailedException(ICommand command, IEnumerable<ValidationResult> results)
        {
            if (command == null) throw new ArgumentNullException("command");
            if (results == null) throw new ArgumentNullException("results");
            Command = command;
            Results = results;
        }

        /// <summary>
        /// Gets a message that describes the current exception.
        /// </summary>
        /// <returns>The error message that explains the reason for the exception, or an empty string("").</returns>
        public override string Message
        {
            get
            {
                string tmp = Command.GetType().Name + " failed.\r\n";
                foreach (var result in Results)
                {
                    tmp += string.Join(", ", result.MemberNames) + ": " + result.ErrorMessage;
                }
                return tmp;
            }
        }
    }
}