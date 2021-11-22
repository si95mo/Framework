using Core;

namespace Hardware.Twincat
{
    /// <summary>
    /// Describe a generic prototype for a Twincat channel
    /// </summary>
    public interface ITwincatChannel : IProperty
    {
        /// <summary>
        /// The Twincat variable name
        /// </summary>
        string VariableName { get; }
    }
}
