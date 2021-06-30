using System;
using System.Linq;

namespace Core.Extensions
{
    /// <summary>
    /// Class that provides useful extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Determine whether an item is contained in a collection of elements
        /// </summary>
        /// <typeparam name="T">The type of the item and the collection</typeparam>
        /// <param name="item">The item to check</param>
        /// <param name="collection">The collection</param>
        /// <returns><see langword="true"/> if the item is contained in the collection,
        /// <see langword="false"/> otherwise</returns>
        public static bool IsIn<T>(this T item, params T[] collection)
        {
            if (item == null)
                throw new ArgumentNullException("The source parameter cannot be null!");

            bool result = collection.Contains(item);

            return result;
        }

        /// <summary>
        /// Format a <see cref="string"/>.
        /// See also <see cref="string.Format(string, object[])"/>
        /// </summary>
        /// <param name="s">The <see cref="string"/> to format</param>
        /// <param name="args">The arguments</param>
        /// <returns>The formatted <see cref="string"/></returns>
        public static string With(this string s, params object[] args)
        {
            string result = string.Format(s, args);

            return result;
        }

        /// <summary>
        /// Determine whether a value is in the range
        /// given (inclusive)
        /// </summary>
        /// <typeparam name="T">The type of the value and the limits</typeparam>
        /// <param name="actual">The value to check</param>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        /// <returns><see langword="true"/> if <paramref name="lower"/> <= <paramref name="actual"/>
        /// <= <paramref name="upper"/>, <see langword="true"/> otherwise</returns>
        public static bool BetweenInclusive<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            bool result = actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) <= 0;

            return result;
        }

        /// <summary>
        /// Determine whether a value is in the range
        /// given (exclusive)
        /// </summary>
        /// <typeparam name="T">The type of the value and the limits</typeparam>
        /// <param name="actual">The value to check</param>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        /// <returns><see langword="true"/> if <paramref name="lower"/> < <paramref name="actual"/>
        /// < <paramref name="upper"/>, <see langword="true"/> otherwise</returns>
        public static bool BetweenExclusive<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            bool result = actual.CompareTo(lower) > 0 && actual.CompareTo(upper) < 0;

            return result;
        }
    }
}
