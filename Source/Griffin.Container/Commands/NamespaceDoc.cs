using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace Griffin.Container.Commands
{
    /// <summary>
    /// Command handling
    /// </summary>
    /// <remarks>Commands should be treated as asynchronous, even if the default implementation is synchronous. That
    /// means that the commands should not return any values. You already got all information and should treat every
    /// command as they where successful. i.e. append the information to the UI. 
    /// <para>If you need IDs, generate them first and then invoke the command.</para></remarks>
    /// <example>
    /// Initialization 
    /// <code>
    /// <![CDATA[
    /// var containerRegistrar = new ContainerRegistrar();
    /// containerRegistrar.RegisterConcrete<ContainerDispatcher>();
    /// var container = containerRegistrar.Build();
    /// ]]>
    /// </code>
    /// Handler:
    /// <code>
    /// <![CDATA[
    /// public class CreateUserHandler : IHandlerOf<CreateUser>
    /// {
    ///     IUserRepository _repos;
    /// 
    ///     public class CreateUserHandler(IUserRepository repository)
    ///     {
    ///         if (repository == null) throw new ArgumentNullException("repository");
    /// 
    ///         _repos = repository;
    ///     }
    /// 
    ///     public void Invoke(InviteUser command)
    ///     {
    ///         var user = _repos.Create(command.FirstName, command.LastName);        
    /// 
    ///         DomainEvent.Publish(new UserCreated(user));
    ///     }
    /// }
    /// ]]>
    /// </code>
    /// Command:
    /// <code>
    /// public class CreateUser : ICommand
    /// {
    ///     public CreateUser(string firstName, string lastName)
    ///     {
    ///         if (firstName == null) throw new ArgumentNullException("firstName");
    ///         if (lastName == null) throw new ArgumentNullException("lastName");
    /// 
    ///         FirstName = firstName;
    ///         LastName = lastName;
    ///     }
    /// 
    ///     public string FirstName { get; private set; }
    ///     public string LastName { get; private set; }
    /// }
    /// </code>
    /// Invocation:
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
    [CompilerGenerated]
    class NamespaceDoc
    {
    }
}
