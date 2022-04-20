using System;
using System.Diagnostics;
using System.Text;

namespace Diagnostic
{
    /// <summary>
    /// Implement a debugger that print to the <see cref="Debug"/> console. <br/>
    /// This class should be intended as a momentary logger to use during debug that prevents several
    /// writing operation to file, but should be then disabled (by setting <see cref="Enabled"/> <see langword="false"/>)
    /// or completely removed from code
    /// </summary>
    public static class Debugger
    {
        /// <summary>
        /// The enable option of the <see cref="Debugger"/>
        /// </summary>
        public static bool Enabled { get; set; } = false;

        /// <summary>
        /// Write a new line in the debug console and append
        /// an <see cref="Environment.NewLine"/> at the end of it if 
        /// <see cref="Enabled"/> is <see langword="true"/>
        /// </summary>
        /// <param name="message">The message to write</param>
        public static void WriteLine(string message)
        {
            if (Enabled)
                Debug.WriteLine($"{Environment.NewLine}>> {DateTime.Now:HH:mm:ss.fff}{Environment.NewLine}\t{message}");
        }

        /// <summary>
        /// Write a <see cref="StackTrace"/> to the debug console and append
        /// an <see cref="Environment.NewLine"/> at the end of it if 
        /// <see cref="Enabled"/> is <see langword="true"/>
        /// </summary>
        /// <param name="stackTrace">The <see cref="StackTrace"/> to write</param>
        public static void WriteLine(StackTrace stackTrace)
            => WriteLine(stackTrace.ToString());

        /// <summary>
        /// Write the <see cref="Exception.Message"/> and eventually the <see cref="Exception.StackTrace"/> to the debug console and
        /// append an <see cref="Environment.NewLine"/> at the end of it if 
        /// <see cref="Enabled"/> is <see langword="true"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to write</param>
        /// <param name="printStackTrace"><see langword="true"/> if <see cref="Exception.StackTrace"/> must be printed, <see langword="false"/> otherwise</param>
        public static void WriteLine(Exception ex, bool printStackTrace)
        {
            string message = ex.Message;

            if (printStackTrace)
                message += $"{Environment.NewLine}\t{FlattenException(ex)}";

            WriteLine(message);
        }

        /// <summary>
        /// Flatten an <see cref="Exception"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to flatten</param>
        /// <returns>The flattened <see cref="Exception"/></returns>
        private static string FlattenException(Exception ex)
        {
            StackTrace stackTrace = new StackTrace(ex, true);
            StackFrame[] frames = stackTrace.GetFrames();
            StringBuilder trace = new StringBuilder();

            foreach (var frame in frames)
            {
                if (frame.GetFileLineNumber() < 1)
                    continue;

                trace.Append("File: " + frame.GetFileName());
                trace.Append(", method:" + frame.GetMethod().Name);
                trace.Append(", lineNumber: " + frame.GetFileLineNumber());
                trace.Append("  -->  ");
            }

            return trace.ToString();
        }
    }
}