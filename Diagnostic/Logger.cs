﻿using IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Diagnostic
{
    /// <summary>
    /// Represent the severity of the entry to log
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// A trace type entry. <br/>
        /// Lowest level of <see cref="Severity"/>
        /// </summary>
        Trace = 0,

        /// <summary>
        /// A debug type entry. <br/>
        /// Higher level of <see cref="Severity"/> than <see cref="Severity.Trace"/>
        /// </summary>
        Debug = 1,

        /// <summary>
        /// An info type entry. <br/>
        /// Higher level of <see cref="Severity"/> than <see cref="Severity.Debug"/>
        /// </summary>
        Info = 2,

        /// <summary>
        /// A warn type entry. <br/>
        /// Higher level of <see cref="Severity"/> than <see cref="Severity.Info"/>
        /// </summary>
        /// <remarks>
        /// This level of <see cref="Severity"/> cannot be disabled.
        /// See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Warn = 3,

        /// <summary>
        /// An error type entry. <br/>
        /// Higher level of <see cref="Severity"/> than <see cref="Severity.Warn"/>
        /// </summary>
        /// <remarks>
        /// This level of <see cref="Severity"/> cannot be disabled.
        /// See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Error = 4,

        /// <summary>
        /// A fatal type entry. <br/>
        /// Highest level of <see cref="Severity"/>
        /// </summary>
        /// <remarks>
        /// This level of <see cref="Severity"/> cannot be disabled.
        /// See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Fatal = 5
    }

    /// <summary>
    /// Class that implements logging functionalities.
    /// </summary>
    public class Logger : FileHandler
    {
        private const string DAILY_SEPARATOR = "****************************************************" +
            "****************************************************";

        private const string ENTRY_SEPARATOR = "----------------------------------------------------" +
            "----------------------------------------------------";

        private const int ENTRY_DESCRITPION_LENGTH = 70; // Header entry description length
        private const int LINE_TYPE_LEGNTH = 6;          // Header type length
        private const int LINE_TIMESTAMP_LENGTH = 23;    // Header timestamp length

        private static string path = "log.txt";
        private static Exception lastException = null;

        private static Severity minimumSeverityLevel = Severity.Debug;

        private static bool initialized = false;

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

        /// <summary>
        /// The minimum <see cref="Severity"/> level of the entry to log
        /// </summary>
        /// <remarks>
        /// Please note that the default minimum level
        /// is <see cref="Severity.Debug"/>
        /// </remarks>
        public static Severity MinimumSeverityLevel => minimumSeverityLevel;

        /// <summary>
        /// The log file path
        /// </summary>
        public static string Path => path;

        /// <summary>
        /// Define whether the <see cref="Logger"/> has been initialized
        /// by calling <see cref="Initialize(string, int)"/> - <see langword="true"/> -
        /// or not - <see langword="false"/>
        /// </summary>
        public static bool Initialized => initialized;

        /// <summary>
        /// Initialize the logger with the specified parameters
        /// </summary>
        /// <remarks>If the <paramref name="timeSpanAsDays"/> is equal to -1 no log file will be deleted (i.e. all
        /// the logs will be kept saved in the disk), otherwise logs older than the actual day
        /// minus the time span specified will be deleted (e.g. if today is 10/01/2021 and
        /// <paramref name="timeSpanAsDays"/> is 10, then all the logs up to
        /// 30/12/2020 will be deleted)</remarks>
        /// <param name="logPath">The path of the log file</param>
        /// <param name="timeSpanAsDays">The time span of daily logs to keep saved (expressed in days); -1 equals no file deleted</param>
        public static void Initialize(string logPath = "logs\\", int timeSpanAsDays = -1)
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd");
            string[] files;
            if (Directory.Exists(logPath))
                files = Directory.GetFiles(logPath);
            else
            {
                Directory.CreateDirectory(logPath);
                files = Directory.GetFiles(logPath);
            }

            if (timeSpanAsDays != -1)
                DeleteOldLogs(timeSpanAsDays, files);

            path = logPath + $"{now}.log";
            IOUtility.CreateDirectoryIfNotExists(logPath);

            string data = null;
            if (File.Exists(path))
                data = Read(path);

            if (data == null || data?.CompareTo("") == 0)
            {
                string applicationName = AppDomain.CurrentDomain.FriendlyName;
                string header = $"{GetDateTime()} - Application: {applicationName}";

                AppendText(header);
                AppendText(DAILY_SEPARATOR);

                string lineTimestamp = "", lineType = "", lineLogEntryDescription = "";
                for (int i = 0; i <= ENTRY_DESCRITPION_LENGTH; i++)
                {
                    if (i <= LINE_TYPE_LEGNTH)
                        lineType += "*";

                    if (i <= LINE_TIMESTAMP_LENGTH)
                        lineTimestamp += "*";

                    lineLogEntryDescription += "*";
                }

                // 23 = LINE_TIMESTAMP_LENGTH
                // 5  = LINE_TYPE_LENGTH - 1
                // 70 = ENTRY_DESCRIPTION_LENGTH
                header = string.Format(
                    "{0, 23}|{1, 5}|{2, 70}",
                    lineTimestamp,
                    lineType,
                    lineLogEntryDescription
                );
                header += Environment.NewLine;
                header += string.Format(
                    "{0, 23} | {1, 5} | {2, 40}",
                    "TIMESTAMP", "TYPE", "LOG ENTRY DESCRIPTION"
                );
                header += Environment.NewLine;
                header += string.Format(
                    "{0, 23}|{1, 5}|{2, 70}",
                    lineTimestamp,
                    lineType,
                    lineLogEntryDescription
                );

                AppendText(header);
            }
            else
                AppendText(DAILY_SEPARATOR);

            initialized = true;
        }

        /// <summary>
        /// Deletes old log files saved in disk
        /// </summary>
        /// <param name="timeSpanAsDays">The time span of file that has to be keep saved (in days)</param>
        /// <param name="files">The <see cref="string"/> array containing the log files path</param>
        private static void DeleteOldLogs(int timeSpanAsDays, string[] files)
        {
            foreach (string file in files)
            {
                string extension = new FileInfo(file).Extension;
                string replace = extension.CompareTo(".log") == 0 ? "_log.log" : "_log.txt";

                string[] str = file.Split('-');
                int year = int.Parse(str[0].Replace("logs\\", ""));
                int month = int.Parse(str[1]);
                int day = int.Parse(str[2].Replace(replace, ""));

                DateTime date = new DateTime(year, month, day);
                int days = DateTime.Now.Subtract(date).Days;

                if (days > timeSpanAsDays)
                    File.Delete(file);
            }
        }

        /// <summary>
        /// Create an entry as a <see cref="Tuple"/>
        /// </summary>
        /// <param name="severity">The <see cref="Severity"/></param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <param name="stackTrace">The <see cref="StackTrace"/> (as <see cref="string"/>)</param>
        /// <returns>The <see cref="Tuple"/> containing the entry</returns>
        private static Tuple<string, string, string, string, string> CreateEntry
            (Severity severity, string source, string message, string stackTrace)
        {
            string severityAsString = GetSeverityAsString(severity);
            string now = GetDateTime();

            var entry = Tuple.Create(now, severityAsString, source, message, stackTrace);

            return entry;
        }

        /// <summary>
        /// Build a new log entry
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <param name="severity">The <see cref="Severity"/> of the entry</param>
        /// <returns>The new entry to log</returns>
        private static string BuildLogEntry(string text, Severity severity)
        {
            string log = $"{GetDateTime()} | {GetSeverityAsString(severity)} | {text}";

            string line = "";

            int counter = 0;
            for (int i = 0; i < log.Length; i++)
            {
                counter++;
                if (counter != 25 && counter != 33)
                    line += "-"; // Normal line separator
                else
                    line += "|"; // Add this char in these position for a table-like appearance
            }

            log += Environment.NewLine + line;

            return log;
        }

        /// <summary>
        /// Build a new log entry
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log</param>
        /// <returns>A <see cref="Tuple{T1, T2, T3, T4, T5}"/> containing the new entry</returns>
        private static Tuple<string, string, string, string, string> BuildLogEntry(Exception ex)
        {
            lastException = ex;

            string message = ex.Message;
            string stackTrace = ex.StackTrace;
            string source = ex.Source;
            string type = ex.GetType().ToString();

            StackTrace st = new StackTrace(ex, true);
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            // Get the file name in which the exception was thrown
            string fileName = frame?.GetFileName();
            // Get the method name
            string methodName = frame?.GetMethod().Name;
            // Get the line number from the stack frame
            int? line = frame?.GetFileLineNumber();

            var entry = CreateEntry(
                Severity.Error,
                $"{source} - {type} on line {line} (method: {methodName}, file: {fileName})",
                message,
                stackTrace
            );

            return entry;
        }

        /// <summary>
        /// Simple log method.
        /// Save the text specified as parameter in the log file.
        /// <see cref="Path"/>
        /// </summary>
        /// <param name="text">The text to be saved</param>
        /// <param name="severity">The <see cref="Severity"/></param>
        public static void Log(string text, Severity severity = Severity.Info)
        {
            if (HasHigherSeverityLevel(severity))
            {
                string log = BuildLogEntry(text, severity);
                AppendText(log);
            }
        }

        /// <summary>
        /// Simple asynchronous log method.
        /// Save the text specified as parameter in the log file.
        /// <see cref="Path"/>
        /// </summary>
        /// <param name="text">The text to be saved</param>
        /// <param name="severity">The <see cref="Severity"/></param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task LogAsync(string text, Severity severity = Severity.Info)
        {
            if (HasHigherSeverityLevel(severity))
            {
                string log = BuildLogEntry(text, severity) + Environment.NewLine;
                await AppendTextAsync(log, hasToAwait: true);
            }
        }

        /// <summary>
        /// Save the text specified as <see cref="Severity.Trace"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Trace(string text)
            => Log(text, Severity.Trace);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Trace"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task TraceAsync(string text)
            => await LogAsync(text, Severity.Trace);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Debug"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Debug(string text)
            => Log(text, Severity.Debug);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Debug"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task DebugAsync(string text)
            => await LogAsync(text, Severity.Debug);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Info"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Info(string text)
            => Log(text, Severity.Info);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Info"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task InfoAsync(string text)
            => await LogAsync(text, Severity.Info);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Warn"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Warn(string text)
            => Log(text, Severity.Warn);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Warn"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task WarnAsync(string text)
            => await LogAsync(text, Severity.Warn);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Error"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <remarks>If the log is required after an <see cref="Exception"/>
        /// occurred, consider using the method <see cref="Log(Exception)"/> instead!</remarks>
        /// <param name="text">The text to log</param>
        public static void Error(string text)
            => Log(text, Severity.Error);

        /// <summary>
        /// Save the <see cref="Exception"/> to the log file.
        /// See also <see cref="Log(Exception)"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> occurred</param>
        public static void Error(Exception ex)
            => Log(ex);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Error"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task ErrorAsync(string text)
            => await LogAsync(text, Severity.Error);

        /// Save the text specified as <see cref="Severity.Fatal"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <remarks>If the log is required after an <see cref="Exception"/>
        /// occurred, consider using the method <see cref="Log(Exception)"/> instead!</remarks>
        /// <param name="text">The text to log</param>
        public static void Fatal(string text)
            => Log(text, Severity.Fatal);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Fatal"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task FatalAsync(string text)
            => await LogAsync(text, Severity.Fatal);

        /// <summary>
        /// Save asynchronously the <see cref="Exception"/> to the log file.
        /// See also <see cref="LogAsync(Exception)"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> occurred</param>
        public static async Task ErrorAsync(Exception ex)
            => await LogAsync(ex);

        /// <summary>
        /// Check if an <see cref="Exception"/> has already been logged
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to check</param>
        /// <returns><see langword="false"/> if an <see cref="Exception"/> has not yet been logged,
        /// <see langword="true"/> otherwise (negated logic)</returns>
        private static bool CheckException(Exception ex)
        {
            bool negatedFlag;

            if (lastException == null) // First exception thrown
            {
                lastException = ex; // lastException == ex -> true
                negatedFlag = false; // force the next if to be true: IsSameExceptionAsTheLast -> true, flag -> true
            }
            else
            {
                negatedFlag = lastException != null; // If lastException != null -> true
            }

            return negatedFlag;
        }

        /// <summary>
        /// Append to the log file a description of the <see cref="Exception"/> occurred
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <remarks>
        /// The entry will be saved <b>only</b> if it differs from
        /// the last one saved in the log file (i.e. different type <b>and</b>
        /// different message <b>and</b> different stack trace)!
        /// </remarks>
        public static void Log(Exception ex)
        {
            bool negatedFlag = CheckException(ex);

            if (!negatedFlag || !IsSameExceptionAsTheLast(ex))
            {
                Tuple<string, string, string, string, string> entry = BuildLogEntry(ex);
                AppendText(entry);
            }
        }

        /// <summary>
        /// Append asynchronously to the log file a description of the <see cref="Exception"/> occurred
        /// </summary>
        /// <param name="ex">The exception to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        /// <remarks>
        /// The entry will be saved <b>only</b> if it differs from
        /// the last one saved in the log file (i.e. different type <b>and</b>
        /// different message <b>and</b> different stack trace)!
        /// </remarks>
        public static async Task LogAsync(Exception ex)
        {
            bool negatedFlag = CheckException(ex);

            if (!negatedFlag || !IsSameExceptionAsTheLast(ex))
            {
                Tuple<string, string, string, string, string> entry = BuildLogEntry(ex);
                await AppendTextAsync(entry);
            }
        }

        /// <summary>
        /// Append text on the log file.
        /// See <see cref="FileHandler.Save(string, string, SaveMode)"/>.
        /// </summary>
        /// <param name="text">The text to append</param>
        private static void AppendText(string text)
            => Save(text, path, SaveMode.Append);

        /// <summary>
        /// Append text on the log file asynchronously.
        /// See <see cref="FileHandler.SaveAsync(string, string, SaveMode)"/>
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="hasToAwait"><see langword="true"/> if the task has to await
        /// for a semaphore, <see langword="false"/> otherwise</param>
        /// <returns>The async <see cref="Task"/></returns>
        private static async Task AppendTextAsync(string text, bool hasToAwait = true)
        {
            if (hasToAwait)
                await semaphore.WaitAsync();

            await SaveAsync(text, path, SaveMode.Append);

            if (hasToAwait)
                semaphore.Release();
        }

        /// <summary>
        /// Append a <see cref="Tuple"/> to the log file as
        /// (timestamp; type of log entry; source; message; stack-trace)
        /// </summary>
        /// <param name="entry">The <see cref="Tuple"/> containing the element to append</param>
        private static void AppendText(Tuple<string, string, string, string, string> entry)
        {
            string text = $"{entry.Item1} | {entry.Item2} | {entry.Item3}{Environment.NewLine}";
            AppendText(text);

            string message = $"\t\tException message: {entry.Item4}{Environment.NewLine}";
            AppendText(message);

            string stackTrace = $"\t\tStack-trace: {entry.Item5}{Environment.NewLine}";
            AppendText(stackTrace);

            AppendText(ENTRY_SEPARATOR + Environment.NewLine);
        }

        /// <summary>
        /// Append asynchronously a <see cref="Tuple"/> to the log file as
        /// (timestamp; type of log entry; source; message; stack-trace)
        /// </summary>
        /// <param name="entry">The <see cref="Tuple"/> containing the element to append</param>
        /// <returns>The async <see cref="Task"/></returns>
        private static async Task AppendTextAsync(Tuple<string, string, string, string, string> entry)
        {
            // Here, hasToAwait has been set to false because the method enter the semaphore once and the release it
            // at the end of all the operations. So, there's no need to await another time inside the AppendTextAsync method!

            await semaphore.WaitAsync();

            string text = $"{entry.Item1} | {entry.Item2} | {entry.Item3}{Environment.NewLine}";
            await AppendTextAsync(text, hasToAwait: false);

            string message = $"\t\tException message: { entry.Item4}{Environment.NewLine}";
            await AppendTextAsync(message, hasToAwait: false);

            string stackTrace = $"\t\tStack-trace: {entry.Item5}{Environment.NewLine}";
            await AppendTextAsync(stackTrace, hasToAwait: false);

            await AppendTextAsync(ENTRY_SEPARATOR + Environment.NewLine, hasToAwait: false);

            semaphore.Release();
        }

        /// <summary>
        /// Convert the <see cref="Severity"/> of the entry to log in a <see cref="string"/>
        /// </summary>
        /// <param name="severity">The <see cref="Severity"/> of the entry</param>
        /// <returns>The <see cref="string"/> result of the conversion</returns>
        private static string GetSeverityAsString(Severity severity)
        {
            string severityAsString = "";

            switch (severity)
            {
                case Severity.Trace:
                    severityAsString = "TRACE";
                    break;

                case Severity.Debug:
                    severityAsString = "DEBUG";
                    break;

                case Severity.Info:
                    severityAsString = "INFO "; // Five letters for alignment!
                    break;

                case Severity.Warn:
                    severityAsString = "WARN "; // Five letters for alignment!
                    break;

                case Severity.Error:
                    severityAsString = "ERROR";
                    break;

                case Severity.Fatal:
                    severityAsString = "FATAL";
                    break;
            }

            return severityAsString;
        }

        /// <summary>
        /// Convert the <see cref="Severity"/> of the entry to log in a readable <see cref="string"/>
        /// (i.e. removing all unnecessary characters as white spaces)
        /// </summary>
        /// <param name="severity"> The severity (<see cref="Severity"/>) of the entry </param>
        /// <returns>The <see cref="string"/> result of the conversion</returns>
        private static string GetReadableSeverityAsString(Severity severity)
        {
            string str = GetSeverityAsString(severity);
            str = new string(
                str.ToCharArray().Where(
                    c => !char.IsWhiteSpace(c)
                ).ToArray()
            );

            return str;
        }

        /// <summary>
        /// Get the <see cref="DateTime"/> in the format "yyyy/MM/dd-HH:mm:ss:fff". <br/>
        /// For example: 2021/02/25-09:32:44:123.
        /// </summary>
        /// <returns>The <see cref="string"/> representing the <see cref="DateTime"/></returns>
        private static string GetDateTime()
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:fff");
            return now;
        }

        /// <summary>
        /// Check if the new <see cref="Exception"/> is the same as the last one, in other words:
        /// <list type="bullet">
        /// <term>they are of the same <see cref="Type"/></term>
        /// </list>
        /// <list type="bullet">
        /// <term>they have the same <see cref="Exception.Message"/></term>
        /// </list>
        /// <list type="bullet">
        /// <term>they have the same <see cref="StackTrace"/></term>
        /// </list>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/>to test </param>
        /// <returns><see langword="true"/> if is the new <see cref="Exception"/> is equal to the last one,
        /// <see langword="false"/> otherwise</returns>
        private static bool IsSameExceptionAsTheLast(Exception ex)
        {
            bool isSameException = ex.GetType() == lastException.GetType();
            isSameException &= ex.Message.CompareTo(lastException.Message) == 0;
            isSameException &= ex.StackTrace == lastException.StackTrace;

            return isSameException;
        }

        /// <summary>
        /// Set the <see cref="MinimumSeverityLevel"/> <see cref="Severity"/> to log.
        /// (i.e. all the entry with a lower severity will not be logged)
        /// </summary>
        /// <remarks>
        /// Note that the minimum level of the logged entry can't be
        /// higher than <see cref="Severity.Info"/> (i.e. entry of level
        /// <see cref="Severity.Warn"/>, <see cref="Severity.Error"/> and
        /// <see cref="Severity.Fatal"/> will always be logged. <br/>
        /// The <see cref="Severity"/> level is defined as follows (from lower to higher):
        /// <see cref="Severity.Trace"/>, <see cref="Severity.Debug"/>,
        /// <see cref="Severity.Info"/>, <see cref="Severity.Warn"/>,
        /// <see cref="Severity.Error"/>, <see cref="Severity.Fatal"/>
        /// </remarks>
        /// <param name="level">The <see cref="Severity"/> level</param>
        public static void SetMinimumSeverityLevel(Severity level)
        {
            Severity oldSeverity = minimumSeverityLevel;

            if ((int)level < (int)Severity.Warn)
                minimumSeverityLevel = level;
            else
                minimumSeverityLevel = Severity.Warn;

            if (oldSeverity != minimumSeverityLevel)
            {
                Log(
                    $"Minimum level set from {GetReadableSeverityAsString(oldSeverity)} " +
                    $"to {GetReadableSeverityAsString(minimumSeverityLevel)}.",
                    severity: Severity.Warn
                );
            }
        }

        /// <summary>
        /// Test if the entry to log has an higher (or equal) <see cref="Severity"/>
        /// level than the <see cref="MinimumSeverityLevel"/> set
        /// </summary>
        /// <param name="level">The <see cref="Severity"/> level to test</param>
        /// <returns><see langword="true"/> of the level to test is higher (or equals)
        /// to <see cref="MinimumSeverityLevel"/>, <see langword="false"/> otherwise</returns>
        private static bool HasHigherSeverityLevel(Severity level)
        {
            bool isHigher = (int)minimumSeverityLevel <= (int)level;
            return isHigher;
        }

        // TODO: Add an async version of all the saving methods of the Logger class. The basic method that should be added is AppendTextAsync
    }
}