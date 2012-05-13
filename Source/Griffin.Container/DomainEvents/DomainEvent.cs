using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Griffin.Container.DomainEvents
{
    /// <summary>
    /// Publishes domain events through the inversion of control container
    /// </summary>
    public class DomainEvent
    {
        static DomainEvent _instance = new DomainEvent();

        /// <summary>
        /// Assign an alternative implementation
        /// </summary>
        /// <param name="alternativeImplementation"></param>
        /// <remarks>The default implementation depends on that you use the default implementation of the ParentContainer: <see cref="Container"/>.</remarks>
        public static void Assign(DomainEvent alternativeImplementation)
        {
            if (alternativeImplementation == null) throw new ArgumentNullException("alternativeImplementation");
            _instance = alternativeImplementation;
        }

        /// <summary>
        /// Publish a domain event
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="domainEvent">The event</param>
        public static void Publish<T>(T domainEvent) where T : class
        {
            _instance.PublishEvent(domainEvent);
        }

        /// <summary>
        /// Publish a domain event
        /// </summary>
        /// <typeparam name="T">Type of event</typeparam>
        /// <param name="domainEvent">The event</param>
        protected virtual void PublishEvent<T>(T domainEvent) where T : class
        {
            if (domainEvent == null) throw new ArgumentNullException("domainEvent");

            if (Container.ChildContainer == null)
                throw new InvalidOperationException("Domain events requires a scoped container.");

            foreach (var subscriber in Container.ChildContainer.ResolveAll<ISubscriberOf<T>>())
            {
                subscriber.Handle(domainEvent);
            }
        }
    }
}
