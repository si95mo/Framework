using IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Diagnostic
{
    /// <summary>
    /// Represent the severity of the entry to log
    /// </summary>
    public enum Severity
    {
        /// <summary>
        /// A trace type entry
        /// </summary>
        Trace = 0,

        /// <summary>
        /// A debug type entry
        /// </summary>
        Debug = 1,

        /// <summary>
        /// An info type entry
        /// </summary>
        Info = 2,

        /// <summary>
        /// A warn type entry
        /// </summary>
        Warn = 3,

        /// <summary>
        /// An error type entry
        /// </summary>
        Error = 4,

        /// <summary>
        /// A fatal type entry
        /// </summary>
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

        private static Severity minimumSeverityLevel = Severity.Info;

        private static bool initialized = false;

        /// <summary>
        /// The minimum <see cref="Severity"/> level of the entry to log
        /// </summary>
        public static Severity MinimumSeverityLevel => minimumSeverityLevel;

        /// <summary>
        /// The log file path
        /// </summary>
        public static string Path => path;

        /// <summary>
        /// Define whether the <see cref="Logger"/> has been initialized
        /// by calling <see cref="Init(string, int)"/> (<see langword="true"/>)
        /// or not (<see langword="false"/>)
        /// </summary>
        public static bool Initialized => initialized;

        /// <summary>
        /// Initialize the logger.
        /// </summary>
        /// <param name="logPath">The path of the log file </param>
        /// <param name="timeSpanAsDays"> The time span of daily log to keep saved (as days interval).
        /// If the parameter value is equal to -1 no log file will be deleted (i.e. all
        /// the logs will be kept saved in the disk).</param>
        public static void Init(string logPath = "logs\\", int timeSpanAsDays = -1)
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

            initialized = true;
        }

        /// <summary>
        /// Deletes old log files saved in disk
        /// </summary>
        /// <param name="timeSpanAsDays">The time span of file that has to be keep saved</param>
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
                string log = $"{GetDateTime()} | {GetSeverityAsString(severity)} | {text}";

                string line = "";

                for (int i = 0; i < log.Length; i++)
                    line += "-";

                log += Environment.NewLine + line;

                AppendText(log);
            }
        }

        /// <summary>
        /// Append to the log file a description of the <see cref="Exception"/> occurred.
        /// The entry will be saved <b>only</b> if its a different one
        /// respect the last one saved in the log file (i.e. different type <b>and</b>
        /// different message <b>and</b> different stack trace)!
        /// </summary>
        /// <param name="ex">The exception to log</param>
        public static void Log(Exception ex)
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

            if (!negatedFlag || !IsSameExceptionAsTheLast(ex))
            {
                lastException = ex;

                string message = ex.Message;
                string stackTrace = ex.StackTrace;
                string source = ex.Source;
                string type = ex.GetType().ToString();

                StackTrace st = new StackTrace(ex, true);
                StackFrame frame = st.GetFrame(st.FrameCount - 1);

                // Get the line number from the stack frame
                int? line = frame?.GetFileLineNumber();

                var entry = CreateEntry(
                    Severity.Error,
                    $"{source} - {type} on line {line}",
                    message,
                    stackTrace
                );

                AppendText(entry);
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
        /// Append a <see cref="Tuple"/> to the log file as
        /// (timestamp; type of log entry; source; message; stack-trace)
        /// </summary>
        /// <param name="entry">The <see cref="Tuple"/> containing the element to append</param>
        private static void AppendText(Tuple<string, string, string, string, string> entry)
        {
            string text = $"{entry.Item1} | {entry.Item2} | {entry.Item3} | {entry.Item4}" +
                $"{Environment.NewLine}";

            AppendText(text);

            string stackTrace = $"\t\tStack-trace: {entry.Item5}{Environment.NewLine}";
            AppendText(stackTrace);

            AppendText(ENTRY_SEPARATOR + Environment.NewLine);
        }

        /// <summary>
        /// Convert the <see cref="Severity"/> of the entry to log in a <see cref="string""/>
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
                    severityAsString = "INFO "; // Five letters for align!
                    break;

                case Severity.Warn:
                    severityAsString = "WARN "; // Five letters for align!
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
        /// Convert the <see cref="Severity"/> of the entry to log in a readable <see cref="string""/>
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
        /// Get the date time in the format "yyyy/MM/dd-HH:mm:ss:fff".
        /// For example: 2021/02/25-09:32:44:000.
        /// </summary>
        /// <returns>The string representing the date time</returns>
        private static string GetDateTime()
        {
            string now = DateTime.Now.ToString("yyyy/MM/dd-HH:mm:ss:fff");
            return now;
        }

        /// <summary>
        /// Check if the new <see cref="Exception"/> is of the same as the last one,
        /// i.e. they are of the same <see cref="Type"/>, they have the same <see cref="Exception.Message"/>
        /// and have the same <see cref="StackTrace"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/>to test </param>
        /// <returns><see langword="true"/> if is the same as the last one,
        /// <see langword="false"/>otherwise</returns>
        private static bool IsSameExceptionAsTheLast(Exception ex)
        {
            bool isSameException = ex.GetType() == lastException.GetType();
            isSameException &= ex.Message.CompareTo(lastException.Message) == 0;
            isSameException &= ex.StackTrace == lastException.StackTrace;

            return isSameException;
        }

        /// <summary>
        /// Set the <see cref="MinimumSeverityLevel"/> <see cref="Severity"/> to log.
        /// (i.e. all the entry with a lower severity will not be logged).
        /// Note that the minimum level of the logged entry can't be
        /// higher than <see cref="Severity.Info"/> (i.e. entry of level
        /// <see cref="Severity.Warn"/>, <see cref="Severity.Error"/> and
        /// <see cref="Severity.Fatal"/>will always be logged.
        /// The <see cref="Severity"/> level is defined as follows (from lower to higher):
        /// <see cref="Severity.Trace"/>, <see cref="Severity.Debug"/>,
        /// <see cref="Severity.Info"/>, <see cref="Severity.Warn"/>,
        /// <see cref="Severity.Error"/>, <see cref="Severity.Fatal"/>
        /// </summary>
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
    }
}