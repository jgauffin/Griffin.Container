using System;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Contract for the dispatcher implementation.
    /// </summary>
    [Obsolete("Use Griffin.Decoupled: http://github.com/jgauffin/griffin.decoupled")]
    public interface ICommandDispatcher
    {
        /// <summary>
        /// Dispatch the command
        /// </summary>
        /// <param name="command">Command to dispatch.</param>
        /// <remarks>A handler MUST exist.</remarks>
        /// <seealso cref="IHandlerOf{T}"/>
        void Dispatch(ICommand command);
    }
}