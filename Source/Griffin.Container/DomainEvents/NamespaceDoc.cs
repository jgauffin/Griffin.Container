using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.DomainEvents
{
    /// <summary>
    /// Small domain event implementation which uses the inverson of control container for the events.
    /// </summary>
    /// <example>
    /// <code>
    /// public class UserService
    /// {
    ///     public void Register(string accountName, string password)
    /// 	{
    /// 		// .. db actions etc ..
    /// 	
    /// 	    DomainEvent.Publish(new UserCreated(user));
    /// 	}
    /// }
    /// 
    /// <![CDATA[
    /// public class UserMailer : ISubscriberOf<UserCreated>
    /// ]]>
    /// {
    ///     public void Handle(UserCreated e)
    /// 	{
    /// 	    _smtpClient.Send(blabla);
    /// 	}
    /// 
    /// }
    /// 
    /// public class UserCreated
    /// {
    /// 	public UserCreated(User user)
    /// 	{
    /// 	     CreatedUser = user;
    /// 	}
    /// 	
    /// 	public User CreatedUser { get; set; }
    /// }
    /// 
    /// 
    ///     /// </code>
    /// </example>
    public class NamespaceDoc
    {

    }
}
