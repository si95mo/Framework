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
        /// <example>
        /// <code>
        /// int item = 10;
        /// if(item.IsIn(0, 5, 2, 4, -6, 10))
        /// {
        ///     // Some stuff
        /// }
        /// else
        /// {
        ///     // Some other stuff
        /// }
        /// </code>
        /// </example>
        public static bool IsIn<T>(this T item, params T[] collection)
        {
            if (item == null)
                throw new ArgumentNullException("The source parameter cannot be null!");

            bool result = collection.Contains(item);
            return result;
        }

        /// <summary>
        /// Format a <see cref="string"/>. <br/>
        /// A syntactic sugar of <see cref="string.Format(string, object[])"/>
        /// </summary>
        /// <param name="s">The <see cref="string"/> to format</param>
        /// <param name="args">The arguments</param>
        /// <returns>The formatted <see cref="string"/></returns>
        /// <example>
        /// <code>
        /// string text = "First: {0}. Second: {1}. Third: {2}".With("1", "2", "3");
        /// // text == "First 1. Second: 2. Third: 3"
        /// </code>
        /// </example>
        public static string With(this string s, params object[] args)
        {
            string result = string.Format(s, args);
            return result;
        }

        /// <summary>
        /// Determine whether a value is in the given range
        /// (inclusive)
        /// </summary>
        /// <typeparam name="T">The type of both the value and the limits</typeparam>
        /// <param name="actual">The value to check</param>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        /// <returns><see langword="true"/> if <paramref name="actual"/> is greater or equal to
        /// <paramref name="lower"/> and lesser or equal to <paramref name="upper"/>,
        /// <see langword="false"/> otherwise</returns>
        /// <example>
        /// <code>
        /// int item = 10;
        /// if(item.IsBetweenInclusive(0, 100)) // True
        /// {
        ///     // Some stuff
        /// }
        /// else
        /// {
        ///     // Some other stuff
        /// }
        /// </code>
        /// </example>
        public static bool IsBetweenInclusive<T>(this T actual, T lower, T upper) where T : IComparable<T>
        {
            bool result = actual.CompareTo(lower) >= 0 && actual.CompareTo(upper) <= 0;
            return result;
        }

        /// <summary>
        /// Determine whether a value is in the given range
        /// (exclusive)
        /// </summary>
        /// <typeparam name="T">The type of the value and the limits</typeparam>
        /// <param name="actual">The value to check</param>
        /// <param name="lower">The lower limit</param>
        /// <param name="upper">The upper limit</param>
        /// <returns><see langword="true"/> if <paramref name="actual"/> is greater than
        /// <paramref name="lower"/> and lesser than <paramref name="upper"/>,
        /// <see langword="true"/> otherwise</returns>
        /// <example>
        /// <code>
        /// int item = 10;
        /// if(item.IsBetweenExclusive(0, 100)) // True
        /// {
        ///     // Some stuff
        /// }
        /// else
        /// {
        ///     // Some other stuff
        /// }
        /// </code>
        /// </example>
        public static bool IsBetweenExclusive<T>(this T actual, T lower, T upper) where T : IComparable<T>
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
        public static T DoIfTrue<T>(this bool value, Func<T> function)
        {
            if (value)
                return function();
            else
                return default;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if <paramref name="value"/> is <see langword="true"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="action">The <see cref="Action"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        /// <example>
        /// <code>
        /// bool flag = true;
        /// int x = 0;
        /// flag.DoIfTrue(() => x += 1);
        /// </code>
        /// </example>
        public static void DoIfTrue(this bool value, Action action)
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
        /// <example>
        /// <code>
        /// bool flag = true;
        /// int x = 0;
        /// flag.DoIfFalse(() => x += 1);
        /// </code>
        /// </example>
        public static T DoIfFalse<T>(this bool value, Func<T> function)
        {
            if (!value)
                return function();
            else
                return default;
        }

        /// <summary>
        /// Perform an <see cref="Action"/> if <paramref name="value"/> is <see langword="false"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="action">The <see cref="Action"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        public static void DoIfFalse(this bool value, Action action)
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
        public static T DoIfTrueIfFalse<T>(this bool value, Func<T> trueFunction, Func<T> falseFunction)
        {
            if (value)
                return trueFunction();
            else
                return falseFunction();
        }

        /// <summary>
        /// Perform an <see cref="Action"/> based on <paramref name="value"/>
        /// </summary>
        /// <typeparam name="T">The type of the value to test</typeparam>
        /// <param name="value">The value</param>
        /// <param name="trueAction">The <see cref="Action"/> to execute
        /// if <paramref name="value"/> is <see langword="true"/></param>
        /// <param name="falseAction">The <see cref="Action"/> to execute
        /// if <paramref name="value"/> is <see langword="false"/></param>
        /// <returns>The result of the <see cref="Action"/></returns>
        public static void DoIfTrueIfFalse(this bool value, Action trueAction, Action falseAction)
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
    /// For the reference article, see this
    /// <a href="http://www.codeproject.com/KB/tips/SerializedObjectCloner.aspx">link</a>
    /// </summary>
    public static class SystemExtension
    {
        /// <summary>
        /// Perform a deep copy of the object via serialization
        /// </summary>
        /// <typeparam name="T">The type of object being copied.</typeparam>
        /// <param name="source">The object instance to copy.</param>
        /// <returns>The deep copy of the object.</returns>
        public static T DeepCopy<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
                throw new ArgumentException("The type must be serializable!", nameof(source));

            // Don't serialize a null object, simply return the default for that object
            if (source == null)
                return default;

            Stream stream = new MemoryStream();
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, source);
            stream.Seek(0, SeekOrigin.Begin);

            return (T)formatter.Deserialize(stream);
        }
    }
}