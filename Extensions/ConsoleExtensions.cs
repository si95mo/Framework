using Diagnostic;
using System;

namespace Extensions
{
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