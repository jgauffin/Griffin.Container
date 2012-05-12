namespace Griffin.Container.Tests
{
    public class Startable2 : ISingletonStartable
    {
        /// <summary>
        /// Invoked when the parent container is created.
        /// </summary>
        public void StartScoped()
        {
            Started = true;
        }

        protected bool Started { get; set; }
    }
}