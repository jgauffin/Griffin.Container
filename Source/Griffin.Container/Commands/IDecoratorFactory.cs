using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Used to decorate command handlers.
    /// </summary>
    public interface IDecoratorFactory
    {
        /// <summary>
        /// Checks if this factory can decorate the specified command
        /// </summary>
        /// <param name="commandType">Type of command being invoked</param>
        /// <returns>true if our decorator can be used; otherwise false.</returns>
        bool CanDecorate(Type commandType);

        /// <summary>
        /// Used to create a new decorator
        /// </summary>
        /// <typeparam name="T">Type of command</typeparam>
        /// <param name="inner"> </param>
        /// <returns>Decorator</returns>
        /// <remarks>The method is only invoked for commands where <see cref="CanDecorate"/> returns <c>true</c>.</remarks>
        IHandlerOf<T> Create<T>(IHandlerOf<T> inner) where T : class, ICommand;
    }
}
