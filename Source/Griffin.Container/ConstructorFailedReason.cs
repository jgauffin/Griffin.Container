using System.Reflection;

namespace Griffin.Container
{
    /// <summary>
    /// A failed constructor and the reason.
    /// </summary>
    public class ConstructorFailedReason
    {
        private readonly string _reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConstructorFailedReason"/> class.
        /// </summary>
        /// <param name="constructor">The constructor.</param>
        /// <param name="reason">Reason why the constructor cant be used.</param>
        public ConstructorFailedReason(ConstructorInfo constructor, string reason)
        {
            _reason = reason;
            Constructor = constructor;
        }

        /// <summary>
        /// Gets tried constructor.
        /// </summary>
        public ConstructorInfo Constructor { get; set; }

        /// <summary>
        /// Gets why constructor cant be used
        /// </summary>
        public string Reason
        {
            get { return _reason; }
        }
    }
}