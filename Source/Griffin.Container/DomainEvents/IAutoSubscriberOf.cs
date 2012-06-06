using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.DomainEvents
{
    /// <summary>
    /// Used to subscribe on domain events
    /// </summary>
    /// <typeparam name="T">Type of event</typeparam>
    public interface ISubscriberOf<in T> where T : class
    {
        /// <summary>
        /// Handle the domain event
        /// </summary>
        /// <param name="e">The event</param>
        void Handle(T e);
    }
}
