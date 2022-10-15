namespace Hardware.Twincat
{
    /// <summary>
    /// Describe a generic Twincat channel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TwincatChannel<T> : Channel<T>, ITwincatChannel
    {
        private string variableName;
        protected IResource resource;

        public string VariableName { get => variableName; protected set => variableName = value; }

        /// <summary>
        /// Create a new instance of <see cref="TwincatChannel{T}"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="variableName">The variable name in Twincat</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        protected TwincatChannel(string code, string variableName, IResource resource, string measureUnit, string format) : base(code, measureUnit, format)
        {
            this.variableName = variableName;
            this.resource = resource;
            this.resource.Channels.Add(this);
        }
    }
}