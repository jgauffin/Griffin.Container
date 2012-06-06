namespace Griffin.Container.Tests
{
    [Component(Lifetime = Lifetime.Singleton)]
    public class OneDepencency
    {
        public OneDepencency(MySelf mySelf)
        {
            
        }
    }
}