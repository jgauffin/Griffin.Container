using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Transactions;

namespace Griffin.Container.DomainEvents
{
    /// <summary>
    /// All domain events are published asynchronously.
    /// </summary>
    /// <remarks>
    /// Events should be async since the operation which generated the event should not
    /// be hindered by faulty subscribers. Instead, the subscriber should log etc.
    /// </remarks>
    public class AsyncDomainEvents : DomainEvent, IDisposable
    {
        private readonly IParentContainer _container;
        private readonly ConcurrentQueue<object> _domainEvents = new ConcurrentQueue<object>();
        private readonly MethodInfo _method;
        private readonly IScopeListener _scopeListener;
        TransactionMonitor _transactionMonitor;
        private long _publishers;


        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncDomainEvents" /> class.
        /// </summary>
        /// <param name="container">Griffin.Container.</param>
        /// <param name="scopeListener">Used to handle errors and scoping setup/teardown.</param>
        public AsyncDomainEvents(IParentContainer container, IScopeListener scopeListener)
        {
            _container = container;
            _scopeListener = scopeListener;
            _method = GetType().GetMethod("TriggerPublish", BindingFlags.NonPublic | BindingFlags.Instance);
            _transactionMonitor = new TransactionMonitor(TriggerEvents);
        }

        private void TriggerEvents(IEnumerable<object> domainEvents)
        {
            foreach (var evt in domainEvents)
            {
                _domainEvents.Enqueue(evt);
            }

            if (Interlocked.Read(ref _publishers) < 5)
                ThreadPool.QueueUserWorkItem(PublishEvents);    
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
        }

        #endregion

        protected override void PublishEvent<T>(T domainEvent)
        {
            if (domainEvent == null) throw new ArgumentNullException("domainEvent");

            if (Transaction.Current != null)
            {
                _transactionMonitor.MonitorEvent(domainEvent);
            }
            else
            {
                _domainEvents.Enqueue(domainEvent);

                if (Interlocked.Read(ref _publishers) < 5)
                    ThreadPool.QueueUserWorkItem(PublishEvents);
            }
        }

        /// <summary>
        /// Invoked through the thread pool to invoke all queued domain events
        /// </summary>
        /// <param name="state"></param>
        protected virtual void PublishEvents(object state)
        {
            Interlocked.Increment(ref _publishers);
            try
            {
                object theEvent;
                if (!_domainEvents.TryDequeue(out theEvent))
                {
                    return;
                }

                _method.MakeGenericMethod(theEvent.GetType()).Invoke(this, new[] {theEvent});
            }
            finally
            {
                Interlocked.Decrement(ref _publishers);
            }
        }

        /// <summary>
        /// Invoked through reflection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="domainEvent"></param>
        protected void TriggerPublish<T>(T domainEvent) where T : class
        {
            using (var child = _container.CreateChildContainer())
            {
                _scopeListener.Started(child);
                var success = true;
                foreach (var subscriber in child.ResolveAll<ISubscriberOf<T>>())
                {
                    try
                    {
                        subscriber.Handle(domainEvent);
                    }
                    catch (Exception err)
                    {
                        _scopeListener.Failure(child, domainEvent, subscriber, err);
                        success = false;
                        break; // do not continue
                    }
                }

                _scopeListener.Ended(child, success);
            }
        }
    }

    public interface IScopeListener
    {
        void Started(IChildContainer scope);
        void Ended(IChildContainer scope, bool wasSucessful);
        void Failure(IChildContainer scope, object domainEvent, object listener, Exception err);
    }
}