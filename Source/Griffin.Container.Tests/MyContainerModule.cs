namespace Griffin.Container.Tests
{
    public class MyContainerModule : IContainerModule
    {
        public void Register(IContainerRegistrar registrar)
        {
            registrar.RegisterConcrete<MySelf>(Lifetime.Transient);
        }
    }
}