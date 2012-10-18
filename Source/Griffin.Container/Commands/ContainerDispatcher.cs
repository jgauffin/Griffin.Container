using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Uses the container to dispatch commands.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <example>
    /// Setup
    /// <code>
    /// <![CDATA[
    /// var registar = new ContainerRegistrar();
    /// registrar.RegisterService<ICommandDispatcher>(x => new ContainerDispatcher(x));
    /// ]]>
    /// </code>
    /// Usage:
    /// <code>
    /// public class UserController : Controller
    /// {
    ///     ICommandDispatcher _dispatcher;
    /// 
    ///     public UserController(ICommandDispatcher dispatcher)
    ///     {
    ///         _dispatcher = dispatcher;
    ///     }
    /// 
    ///     public ActionResult Create(CreateModel model)
    ///     {
    ///         if (!ModelState.IsValid)
    ///             return View(model);
    /// 
    ///         _dispatcher.Dispatch(new CreateUser(model.FirstName, model.LastName));
    ///         TempData.Add("Message", "User have been created");
    ///         return RedirectToAction("List");
    ///     }
    /// }
    /// </code>
    /// </example>
    [Obsolete("Use Griffin.Decoupled: http://github.com/jgauffin/griffin.decoupled")]
    public class ContainerDispatcher : ICommandDispatcher
    {
        private readonly IServiceLocator _serviceLocator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerDispatcher"/> class.
        /// </summary>
        /// <param name="serviceLocator">The service locator. The container implements the interface.</param>
        public ContainerDispatcher(IServiceLocator serviceLocator)
        {
            if (serviceLocator == null) throw new ArgumentNullException("serviceLocator");
            _serviceLocator = serviceLocator;
        }

        /// <summary>
        /// Dispatch the command
        /// </summary>
        /// <param name="command">Command to invoke</param>
        public void Dispatch(ICommand command)
        {
            if (command == null) throw new ArgumentNullException("command");

            var handlerType = typeof(IHandlerOf<>).MakeGenericType(command.GetType());
            var method = handlerType.GetMethod("Invoke", new Type[] { command.GetType() });

            var handler = _serviceLocator.Resolve(handlerType);
            var decorated = Decorate(command.GetType(), handler);

            // workaround so that TargetInvocationException is not thrown.
            ((dynamic) decorated).Invoke((dynamic)command);
        }

        /// <summary>
        /// Gets the generic Create method from the decorate.
        /// </summary>
        /// <param name="commandType">Type of the command.</param>
        /// <returns></returns>
        public MethodInfo GetDecoratorCreateMethod(Type commandType)
        {
            var factoryType = typeof (IDecoratorFactory);
            var methodInfo = factoryType.GetMethod("Create");
            if (methodInfo == null)
            {
                foreach (var method in factoryType.GetMethods())
                {
                    if (method.Name != "Invoke" || !method.IsGenericMethod || method.GetParameters().Length != 1)
                        continue;

                    methodInfo = method;
                    break;
                }
            }

            if (methodInfo == null)
                throw new InvalidOperationException("Failed to find 'Create' method.");

            return methodInfo.MakeGenericMethod(commandType);
        }

        /// <summary>
        /// Checks if the current handler can be decorated
        /// </summary>
        /// <param name="commandType">Type of command (implementation of <see cref="ICommand"/>)</param>
        /// <param name="handler">Current handler</param>
        /// <returns><paramref name="handler"/> if no decorators are found; otherwise the last decorator.</returns>
        protected virtual object Decorate(Type commandType, object handler)
        {
            if (commandType == null) throw new ArgumentNullException("commandType");
            if (handler == null) throw new ArgumentNullException("handler");


            var decoratorMethod = GetDecoratorCreateMethod(commandType);

            var result = handler;
            foreach (var decoratorFactory in _serviceLocator.ResolveAll<IDecoratorFactory>())
            {
                if (!decoratorFactory.CanDecorate(commandType))
                    continue;

                result = decoratorMethod.Invoke(decoratorFactory, new[] {result});
            }

            return result;
        }
    }
}