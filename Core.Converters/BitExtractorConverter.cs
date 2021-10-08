using System;

namespace Core.Converters
{
    public class BitExtractorConverter : AbstractConverter<double, bool>
    {
        /// <summary>
        /// Create a new instance of <see cref="BitExtractorConverter"/>
        /// </summary>
        /// <param name="position">The bit position</param>
        public BitExtractorConverter(int position) : base()
        {
            converter = new Func<double, bool>(x => ExtractBits(x, 1, position) != 0);
        }

        /// <summary>
        /// Extract a specified number of bits from a number, starting from a position
        /// </summary>
        /// <param name="number">The number</param>
        /// <param name="numberOfBits">The number of bits to extract</param>
        /// <param name="position">The starting position</param>
        /// <returns>The extracted bits</returns>
        private int ExtractBits(double number, int numberOfBits, int position)
        {
            int shiftedNumber = (1 << numberOfBits) - 1;
            int result = shiftedNumber & ((int)number >> position);

            return result;
        }
    }
}