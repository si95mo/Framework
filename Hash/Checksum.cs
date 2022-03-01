using System.Linq;

namespace Hash
{
    /// <summary>
    /// Class that calculate the CRC of a <see cref="string"/>
    /// </summary>
    public static class Checksum
    {
        /// <summary>
        /// Calculate the checksum of a string by adding all the ASCII code
        /// elements and converting the sum from base 10 to hex.
        /// The checksum consists of the last 2 digits of the hex number.
        /// </summary>
        /// <param name="input">The string of which calculate the checksum</param>
        /// <returns>The checksum, calculated as previously described</returns>
        public static string CreateNew(string input)
        {
            int sum = 0;
            for (int i = 0; i < input.Count(); i++)
                sum += input[i];

            string checksum = sum.ToString("X");
            checksum = "" + checksum.Substring(checksum.Length - 2);

            return checksum;
        }
    }
}