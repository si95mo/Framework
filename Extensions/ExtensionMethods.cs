using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Extensions
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

        /// <summary>
        /// Perform a <see cref="Func{T, TResult}"/> if <paramref name="value"/> is <see langword="true"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="function">The <see cref="Func{T, TResult}"/></param>
        /// <returns>The result of the <see cref="Func{T, TResult}"/></returns>
        public static T IfTrue<T>(this bool value, Func<T> function)
        {
            if (value)
                return (T)function();
            else
                return default(T);
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if <paramref name="value"/> is <see langword="true"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="action">The <see cref="Action"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        public static void IfTrue(this bool value, Action action)
        {
            if (value)
                action();
        }

        /// <summary>
        /// Perform a <see cref="Func{T, TResult}"/> if <paramref name="value"/> is <see langword="false"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="function">The <see cref="Func{T, TResult}"/></param>
        /// <returns>The result of the <see cref="Func{T, TResult}"/></returns>
        public static T IfFalse<T>(this bool value, Func<T> function)
        {
            if (!value)
                return (T)function();
            else
                return default(T);
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if <paramref name="value"/> is <see langword="false"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="action">The <see cref="Action"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        public static void IfFalse(this bool value, Action action)
        {
            if (!value)
                action();
        }

        /// <summary>
        /// Perform a <see cref="Func{T, TResult}"/> based on <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="trueFunction">The <see cref="Func{T, TResult}"/> to execute
        /// if <paramref name="value"/> is <see langword="true"/></param>
        /// <param name="falseFunction">The <see cref="Func{T, TResult}"/> to execute
        /// if <paramref name="value"/> is <see langword="false"/></param>
        /// <returns>The result of the <see cref="Func{T, TResult}"/></returns>
        public static T IfTrueIfFalse<T>(this bool value, Func<T> trueFunction, Func<T> falseFunction)
        {
            if (value)
                return (T)trueFunction();
            else
                return (T)falseFunction();
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if <paramref name="value"/> is <see langword="false"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="trueAction">The <see cref="Action"/> to execute
        /// if <paramref name="value"/> is <see langword="true"/></param>
        /// <param name="falseAction">The <see cref="Action"/> to execute
        /// if <paramref name="value"/> is <see langword="false"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        public static void IfTrueIfFalse(this bool value, Action trueAction, Action falseAction)
        {
            if (value)
                trueAction();
            else
                falseAction();
        }
    }

    /// <summary>
    /// Provides a method for performing a deep copy of an object.
    /// Binary Serialization is used to perform the copy.
    /// </summary>
    // Reference Article http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx
    public static class SystemExtension
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization.
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>A deep copy of the object.</returns>
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable.", nameof(source));

            // Don't serialize a null object, simply return the default for that object
            if (ReferenceEquals(source, null))
                return default;

            Stream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);

            return (T)formatter.Deserialize(stream);
        }
    }
}