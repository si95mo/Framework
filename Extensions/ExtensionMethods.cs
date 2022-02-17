using Dasync.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Class that provides useful extension methods, not related to a particular field
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Perform an async for each, in parallel
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="function">The <see cref="Func{T, TResult}"/> to execute</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task ForEachAsync<T>(this IEnumerable<T> source, Func<T, Task> function)
            => await source.ParallelForEachAsync(function);

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
}