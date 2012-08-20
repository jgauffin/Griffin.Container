namespace Griffin.Container.Tests.Subjects
{
    public class Startable1 : ISingletonStartable
    {
        /// <summary>
        /// Invoked when the parent container is created.
        /// </summary>
        public void StartSingleton()
        {
            Started = true;
        }

        protected bool Started { get; set; }
    }
}