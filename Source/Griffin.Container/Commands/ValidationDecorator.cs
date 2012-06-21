using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Uses data annotation to validate commands.
    /// </summary>
    /// <typeparam name="T">Type of command</typeparam>
    /// <remarks>Throws <see cref="CommandValidationFailedException"/> if validation fails.</remarks>
    public class ValidationDecorator<T> : IHandlerOf<T> where T : class, ICommand
    {
        private readonly IHandlerOf<T> _inner;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationDecorator&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="inner">The inner.</param>
        public ValidationDecorator(IHandlerOf<T> inner)
        {
            _inner = inner;
        }

        /// <summary>
        /// Invokes the specified command.
        /// </summary>
        /// <param name="command">The command.</param>
        public void Invoke(T command)
        {
            var context = new ValidationContext(command, null, null);
            var results = new List<ValidationResult>();

            var isValid = Validator.TryValidateObject(command, context, results);
            if (!isValid)
                throw new CommandValidationFailedException(command, results);

            _inner.Invoke(command);
        }
    }
}
