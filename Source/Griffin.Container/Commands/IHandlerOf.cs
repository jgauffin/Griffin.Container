namespace Griffin.Container.Commands
{
    /// <summary>
    /// Used to handle comands.
    /// </summary>
    /// <typeparam name="T">Type of command which is handled</typeparam>
    /// <remarks>Don't let a class implement several handlers. Keep the classes simple with a single responsibility.
    /// It allows you to decorate the handlers and control common tasks like exception logging (together with all command parameters).</remarks>
    public interface IHandlerOf<in T> where T : class, ICommand
    {
        /// <summary>
        /// Invoke the command
        /// </summary>
        /// <param name="command">Command</param>
        void Invoke(T command);
    }
}