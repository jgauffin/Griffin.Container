namespace Griffin.Container.Interception
{
    public class CallContext
    {
        public object[] Arguments { get; set; }
        public object ReturnValue { get; set; }
        public object Instance { get; set; }

    }
}