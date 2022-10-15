namespace Hardware.Twincat
{
    /// <summary>
    /// Describe a generic Twincat channel
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TwincatChannel<T> : Channel<T>, ITwincatChannel
    {
        private string variableName, arrayName;
        private int positionInArray;
        protected IResource resource;

        public string VariableName { get => variableName; protected set => variableName = value; }
        public string ArrayName { get => arrayName; protected set => arrayName = value; }
        public int PositionInArray { get => positionInArray; protected set => positionInArray = value; }

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
            if(variableName.Contains("[")) // Array
            {
                int indexOfFirstParenthesis = variableName.IndexOf("[");
                int indexOfSecondParenthesis = variableName.IndexOf("]");

                ArrayName = variableName.Substring(0, indexOfFirstParenthesis);
                int.TryParse(variableName.Substring(indexOfFirstParenthesis, indexOfSecondParenthesis - indexOfFirstParenthesis - 1), out int index);
                PositionInArray = index;
            }
            else
            {
                arrayName = string.Empty;
                positionInArray = -1;
            }

            this.resource = resource;

            this.resource.Channels.Add(this);
        }
    }
}