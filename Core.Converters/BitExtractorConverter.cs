using System;

namespace Core.Converters
{
    /// <summary>
    /// Implement an <see cref="AbstractConverter{TIn, TOut}"/> that
    /// extract a single bit from a <see cref="double"/>
    /// </summary>
    public class BitExtractorConverter : AbstractConverter<double, bool>
    {
        /// <summary>
        /// Create a new instance of <see cref="BitExtractorConverter"/>
        /// </summary>
        /// <param name="position">The bit position</param>
        public BitExtractorConverter(int position) : base()
        {
            converter = new Func<double, bool>(x => ExtractBit(x, 1, position) != 0);
        }

        /// <summary>
        /// Extract a specified number of bits from a number, starting from a position
        /// </summary>
        /// <param name="number">The number</param>
        /// <param name="numberOfBits">The number of bits to extract</param>
        /// <param name="position">The starting position</param>
        /// <returns>The extracted bits</returns>
        private int ExtractBit(double number, int numberOfBits, int position)
        {
            int shiftedNumber = (1 << numberOfBits) - 1;
            int result = shiftedNumber & ((int)number >> position);

            return result;
        }
    }
}