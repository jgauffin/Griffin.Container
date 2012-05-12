using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// A failed constructor and the reason.
    /// </summary>
    public class ConstructorFailedReason
    {
        private readonly string _reason;
        public ConstructorInfo Constructor { get; set; }

        public ConstructorFailedReason(ConstructorInfo constructor, string reason)
        {
            _reason = reason;
            Constructor = constructor;
        }
    }
}