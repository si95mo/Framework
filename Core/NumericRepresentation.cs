namespace Core
{
    /// <summary>
    /// Define the numeric representation of variables
    /// </summary>
    public enum NumericRepresentation
    {
        /// <summary>
        /// An <see cref="ushort"/> type of variable
        /// (16 bit unsigned integer value)
        /// </summary>
        UInt16 = 0,

        /// <summary>
        /// An <see cref="int"/> type of variable
        /// (32 bit integer value)
        /// </summary>
        Int32 = 1,

        /// <summary>
        /// A <see cref="float"/> type of variable
        /// (single precision value - IEEE754 standard for 32 bit)
        /// </summary>
        Single = 2,

        /// <summary>
        /// A <see cref="double"/> type of variable
        /// (double precision value - IEEE754 standard for 64 bit)
        /// </summary>
        Double = 3,

        /// <summary>
        /// A <see cref="bool"/> type of variable
        /// (<see langword="true"/> or <see langword="false"/> value)
        /// </summary>
        Boolean = 4
    }
}