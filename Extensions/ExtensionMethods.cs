using Dasync.Collections;
using Diagnostic;
using OX.Copyable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Class that provides useful extension methods
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Perform an async for each, in parallel
        /// </summary>
        /// <param name="source">The source collection</param>
        /// <param name="function">The <see cref="Func{T, TResult}"/> to execute</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task ForEachAsync(this IEnumerable<object> source, Func<object, Task> function)
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

    /// <summary>
    /// Provides a method for performing a deep copy of an object. <br/>
    /// Reflection is used to perform the deep copy
    /// </summary>
    /// <remarks>
    /// Reflection is used instead of a binary deep copy
    /// for security reasons. <br/>
    /// In fact, the MSDN binary formatter will gradually be
    /// removed in following .NET releases
    /// </remarks>
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
            try
            {
                object copy = source.Copy();
                return (T)copy;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
                return default;
            }
        }
    }

    /// <summary>
    /// Implements <see cref="Console"/> extensions
    /// </summary>
    public static class ConsoleExtension
    {
        /// <summary>
        /// Clear the <see cref="Console"/>
        /// </summary>
        public static void Clear() => Console.Clear();

        /// <summary>
        /// Capture the enter key submitted by the user
        /// in the <see cref="Console"/>
        /// </summary>
        public static void AcquireEnter() => Console.ReadLine();

        /// <summary>
        /// Set the <see cref="Console"/> <see cref="ConsoleColor"/>
        /// </summary>
        /// <param name="color">The <see cref="ConsoleColor"/></param>
        public static void SetConsoleTextColor(ConsoleColor color) => Console.ForegroundColor = color;

        /// <summary>
        ///  Reset the <see cref="Console"/> <see cref="ConsoleColor"/>
        /// </summary>
        public static void ResetConsoleTextColor() => Console.ResetColor();

        /// <summary>
        /// Acquire the user input from the <see cref="Console"/>
        /// as <see cref="char"/>
        /// </summary>
        /// <returns>The user input</returns>
        /// <remarks>
        /// The method returns <see langword="\0"/> if an
        /// incorrect input is submitted
        /// </remarks>
        public static char AcquireUserInputAsChar()
        {
            char userInput = '\0';

            SetConsoleTextColor(ConsoleColor.Yellow);
            try
            {
                userInput = Convert.ToChar(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            ResetConsoleTextColor();

            return userInput;
        }

        /// <summary>
        /// Acquire the user input from the <see cref="Console"/>
        /// as <see cref="int"/>
        /// </summary>
        /// <param name="numberBase">The number base</param>
        /// <returns>The user input</returns>
        /// <remarks>
        /// The method returns <see cref="int.MaxValue"/> if an
        /// incorrect input is submitted
        /// </remarks>
        public static int AcquireUserInputAsInt(int numberBase = 10)
        {
            int userInput = int.MaxValue;
            SetConsoleTextColor(ConsoleColor.Yellow);
            try
            {
                userInput = Convert.ToInt32(Console.ReadLine(), numberBase);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
            ResetConsoleTextColor();

            return userInput;
        }

        /// <summary>
        /// Acquire the user input from the <see cref="Console"/>
        /// as <see cref="uint"/>
        /// </summary>
        /// <param name="numberBase">The number base</param>
        /// <returns>The user input</returns>
        /// <remarks>
        /// The method returns <see cref="uint.MaxValue"/> if an
        /// incorrect input is submitted
        /// </remarks>
        public static uint AcquireUserInputAsUInt(int numberBase = 10)
        {
            uint userInput = uint.MaxValue;
            SetConsoleTextColor(ConsoleColor.Yellow);
            try
            {
                userInput = Convert.ToUInt32(Console.ReadLine(), numberBase);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            ResetConsoleTextColor();

            return userInput;
        }

        /// <summary>
        /// Acquire the user input from the <see cref="Console"/>
        /// as <see cref="double"/>
        /// </summary>
        /// <returns>The user input</returns>
        /// <remarks>
        /// The method returns <see cref="double.MaxValue"/> if an
        /// incorrect input is submitted
        /// </remarks>
        public static double AcquireUserInputAsDouble()
        {
            double userInput = double.MaxValue;
            SetConsoleTextColor(ConsoleColor.Yellow);
            try
            {
                userInput = Convert.ToDouble(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message);
            }
            ResetConsoleTextColor();

            return userInput;
        }

        /// <summary>
        /// Set the <see cref="Console"/> size
        /// </summary>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        public static void SetConsoleSize(int width, int height)
            => Console.SetWindowSize(width, height);

        /// <summary>
        /// Set the <see cref="Console"/> position
        /// </summary>
        /// <param name="left">The distance from the left corner</param>
        /// <param name="top">The distance from the top corner</param>
        public static void SetConsolePosition(int left, int top)
            => Console.SetWindowPosition(left, top);
    }
}