using System;

namespace Griffin.Container.Tests
{
    [Component]
    public class MySelf : IDisposable
    {
        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <filterpriority>2</filterpriority>
        public void Dispose()
        {
            IsDisposed = true;
        }

        public bool IsDisposed { get; set; }
    }
}