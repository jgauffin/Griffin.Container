namespace Griffin.Container.Interception
{
    /// <summary>
    /// Call context for intercepted method
    /// </summary>
    public class CallContext
    {
        /// <summary>
        /// Gets or set method arguments
        /// </summary>
        public object[] Arguments { get; set; }

        /// <summary>
        /// Gets or sets returned value
        /// </summary>
        public object ReturnValue { get; set; }

        /// <summary>
        /// Gets or sets class instance.
        /// </summary>
        public object Instance { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("{0}({1})", Instance.GetType().FullName, string.Join(", ", Arguments));
        }
    }
}