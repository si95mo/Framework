using IO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic
{
    /// <summary>
    /// Represent the severity of the entry to log
    /// </summary>
    public enum SeverityType
    {
        /// <summary>
        /// An info type entry
        /// </summary>
        INFO,
        /// <summary>
        /// A debug type entry
        /// </summary>
        DEBUG,
        /// <summary>
        /// A warn type entry
        /// </summary>
        WARN,
        /// <summary>
        /// An error type entry
        /// </summary>
        ERROR
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

        private static string path = "log.txt";
        private static Exception lastException = null;

        /// <summary>
        /// Getter for the log file path
        /// </summary>
        public static string Path { get => path; }

        /// <summary>
        /// Initialize the logger.
        /// </summary>
        /// <param name="logPath"> The path of the log file </param>
        /// <param name="timeSpanAsDays"> The number of daily logs to keep saved.
        /// If the parameter value is equal to -1 no log file will be deleted (i.e. all
        /// the logs will be kept saved in the disk). </param>
        public static void Init(string logPath = "logs\\", int timeSpanAsDays = -1)
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd");
            string[] files;
            if(Directory.Exists(logPath))
                files = Directory.GetFiles(logPath);
            else
            {
                Directory.CreateDirectory(logPath);
                files = Directory.GetFiles(logPath);
            }

            if (timeSpanAsDays != -1)
                DeleteOldLogs(timeSpanAsDays, files);

            path = logPath + $"{now}.log";
            Utility.CreateDirectoryIfNotExists(logPath);

            string data = null;
            if (File.Exists(Path))
                data = Read(path);

            if (data == null || data?.CompareTo("") == 0)
            {
                string applicationName = AppDomain.CurrentDomain.FriendlyName;
                string header = $"{GetDateTime()} - Application: {applicationName}";

                AppendText(header);
                AppendText(DAILY_SEPARATOR);

                string lineTimestamp = "", lineType = "", lineLogEntryDescription = "";
                for(int i = 0; i <= 70; i++)
                {
                    if (i <= 6)
                        lineType += "*";

                    if (i <= 23)
                        lineTimestamp += "*";

                    lineLogEntryDescription += "*";
                }

                header = string.Format(
                    "{0, 23}|{1, 5}|{2, 70}", 
                    lineTimestamp, 
                    lineType,
                    lineLogEntryDescription
                );
                header += Environment.NewLine;
                header += string.Format("{0, 23} | {1, 5} | {2, 40}", "TIMESTAMP", "TYPE", "LOG ENTRY DESCRIPTION");
                header += Environment.NewLine;
                header += string.Format(
                    "{0, 23}|{1, 5}|{2, 70}",
                    lineTimestamp,
                    lineType,
                    lineLogEntryDescription
                );

                AppendText(header);
            }
        }

        /// <summary>
        /// Deletes old log files saved in disk
        /// </summary>
        /// <param name="timeSpanAsDays"> The time span of file that has to be keep saved </param>
        /// <param name="files"> The <see cref="string"/> array containing the log files path </param>
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
        /// Create an header as a <see cref="Tuple"/>
        /// </summary>
        /// <returns>The <see cref="IEnumerable{T}"/> <see cref="Tuple"/></returns>
        private IEnumerable<Tuple<string, string, string, string>> CreateHeader()
        {
            var header = new[]{
                Tuple.Create("Timestamp", "Severity", "Source", "Message")
            };

            return header;
        }

        /// <summary>
        /// Create an entry as a <see cref="Tuple"/>
        /// </summary>
        /// <param name="timestamp">The timestamp</param>
        /// <param name="severity">The <see cref="SeverityType"/></param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <returns>The <see cref="IEnumerable{T}"/> <see cref="Tuple"/></returns>
        private static IEnumerable<Tuple<string, string, string, string>> CreateEntry
            (string timestamp, SeverityType severity, string source, string message)
        {
            string severityAsString = GetSeverityAsString(severity);

            string now = GetDateTime();

            var entry = new[]{
                Tuple.Create(now, severityAsString, source, message)
            };

            return entry;
        }

        /// <summary>
        /// Create an entry as a <see cref="Tuple"/>
        /// </summary>
        /// <param name="severity">The <see cref="SeverityType"/></param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <param name="stackTrace">The <see cref="StackTrace"/> (as <see cref="string"/>)</param>
        /// <returns></returns>
        private static Tuple<string, string, string, string, string> CreateEntry
            (SeverityType severity, string source, string message, string stackTrace)
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
        /// <param name="text"> The text to be saved </param>
        /// <param name="severity">The <see cref="SeverityType"/></param>
        public static void Log(string text, SeverityType severity)
        {
            string log = $"{GetDateTime()} | {GetSeverityAsString(severity)} | {text}";

            string line = "";

            for (int i = 0; i < log.Length; i++)
                line += "-";

            log += Environment.NewLine + line;

            AppendText(log);
        }

        /// <summary>
        /// Append to the log file a description of the exception occurred.
        /// The entry will be saved <b>only</b> if its a different one
        /// respect the last one saved in the log file (i.e. different type <b>and</b> 
        /// different message <b>and</b> different stack trace)!
        /// </summary>
        /// <param name="ex"> The exception to log </param>
        public static void Log(Exception ex)
        {
            bool negatedFlag;

            if(lastException == null) // First exception thrown
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
                int line = frame.GetFileLineNumber();

                var entry = CreateEntry(
                    SeverityType.ERROR, 
                    $"{source} - {type} on line {line}", 
                    message, 
                    stackTrace
                );

                AppendText(entry);
            }
        }

        /// <summary>
        /// Append text on the log file. 
        /// See <see cref="FileHandler.Save(string, string, MODE)"/>.
        /// </summary>
        /// <param name="text"> The text to append </param>
        private static void AppendText(string text)
            => Save(text, path, MODE.APPEND);

        /// <summary>
        /// Append a tuple to the log file as
        /// (timestamp; type of log entry; source; message).
        /// See <see cref="AppendText(string)"/>
        /// </summary>
        /// <param name="entry"> The tuple containing the elements to append </param>
        private static void AppendText(Tuple<string, string, string, string> entry)
        {
            string text = $"{entry.Item1} | {entry.Item2} | {entry.Item3} | {entry.Item4}" +
                $"{Environment.NewLine}";

            AppendText(text);
            AppendText(ENTRY_SEPARATOR + Environment.NewLine);
        }

        /// <summary>
        /// Append a tupe to the log file as
        /// (timestamp; type of log entry; source; message; stack-trace)
        /// </summary>
        /// <param name="entry"> The tuple containing the element to append </param>
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
        /// Convert the <see cref="SeverityType"/> of the entry to lo in a <see cref="string""/>
        /// </summary>
        /// <param name="severity"> The severity (<see cref="SeverityType"/>) of the entry </param>
        /// <returns></returns>
        private static string GetSeverityAsString(SeverityType severity)
        {
            string severityAsString = "";

            switch (severity)
            {
                case SeverityType.DEBUG:
                    severityAsString = "DEBUG";
                    break;
                case SeverityType.ERROR:
                    severityAsString = "ERROR";
                    break;
                case SeverityType.INFO:
                    severityAsString = "INFO "; // Five letters for align!
                    break;
                case SeverityType.WARN:
                    severityAsString = "WARN "; // Five letters for align!
                    break;
            }

            return severityAsString;
        }

        /// <summary>
        /// Get the date time in the format "yyyy/MM/dd-HH:mm:ss:fff".
        /// For example: 2021/02/25-09:32:44:000.
        /// </summary>
        /// <returns> The string representing the date time </returns>
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
        /// <param name="ex"> The <see cref="Exception"/> to test </param>
        /// <returns> <see langword="true"/> if is the same as the last one, 
        /// <see langword="false"/> otherwise</returns>
        private static bool IsSameExceptionAsTheLast(Exception ex)
        {
            bool isSameException = ex.GetType() == lastException.GetType();
            isSameException &= ex.Message.CompareTo(lastException.Message) == 0;
            isSameException &= ex.StackTrace == lastException.StackTrace;

            return isSameException;
        }
    }
}
