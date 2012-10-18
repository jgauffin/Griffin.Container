using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Facade for the command handling
    /// </summary>
    /// <remarks>You need to assign a handler using the <see cref="Assign"/> method. Then use <see cref="Dispatch"/> to send the commands.
    /// There is no guarantee when a command is processed. Do not expect a response.</remarks>
    [Obsolete("Use Griffin.Decoupled: http://github.com/jgauffin/griffin.decoupled")]
    public class CommandDispatcher
    {
        private static ICommandDispatcher _instance;

        /// <summary>
        /// Assign an implementation.
        /// </summary>
        /// <param name="dispatcher">Dispatcher to invoke.</param>
        public static void Assign(ICommandDispatcher dispatcher)
        {
            if (dispatcher == null) throw new ArgumentNullException("dispatcher");
            _instance = dispatcher;
        }

        /// <summary>
        /// Fire away a command
        /// </summary>
        /// <param name="command">Command to invoke</param>
        public static void Dispatch(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");
            _instance.Dispatch(command);
        }
    }
}
