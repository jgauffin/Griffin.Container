namespace Griffin.Container.Tests
{
    public class MyContainerModule : IContainerModule
    {
        public void Register(IContainerRegistrar registrar)
        {
            registrar.RegisterType<MySelf>(Lifetime.Transient);
        }
    }
}