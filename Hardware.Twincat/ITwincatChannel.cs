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

        /// <summary>
        /// The array name (if in an array, <see cref="string.Empty"/> otherwise)
        /// </summary>
        string ArrayName { get; }

        /// <summary>
        /// The position inside the array (if in an array, -1 otherwise)
        /// </summary>
        int PositionInArray { get; }
    }
}