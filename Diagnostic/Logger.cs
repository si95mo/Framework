using IO;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
        /// This level of <see cref="Severity"/> cannot be disabled. See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Warn = 3,

        /// <summary>
        /// An error type entry. <br/>
        /// Higher level of <see cref="Severity"/> than <see cref="Severity.Warn"/>
        /// </summary>
        /// <remarks>
        /// This level of <see cref="Severity"/> cannot be disabled. See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Error = 4,

        /// <summary>
        /// A fatal type entry. <br/>
        /// Highest level of <see cref="Severity"/>
        /// </summary>
        /// <remarks>
        /// This level of <see cref="Severity"/> cannot be disabled. See <see cref="Logger.SetMinimumSeverityLevel(Severity)"/>
        /// </remarks>
        Fatal = 5
    }

    /// <summary>
    /// Class that implements logging functionalities.
    /// </summary>
    public class Logger : FileHandler
    {
        #region Private data structures

        /// <summary>
        /// Represent an <see cref="Exception"/> entry to be logged
        /// </summary>
        private struct ExceptionEntry
        {
            /// <summary>
            /// The timestamp
            /// </summary>
            public string Timestamp;

            /// <summary>
            /// The <see cref="Diagnostic.Severity"/> as <see cref="string"/>
            /// </summary>
            public string Severity;

            /// <summary>
            /// The <see cref="Exception.Source"/>
            /// </summary>
            public string Source;

            /// <summary>
            /// The <see cref="Exception.Message"/>
            /// </summary>
            public string Message;

            /// <summary>
            /// The <see cref="Exception.StackTrace"/> as <see cref="string"/>
            /// </summary>
            public string StackTrace;

            public override string ToString()
            {
                string header = $"{Timestamp} | {Severity} | {Source}{Environment.NewLine}";
                string message = $"\t\tException message: {Message}{Environment.NewLine}";
                string stackTrace = $"\t\tStack-trace: {StackTrace}{Environment.NewLine}";

                string description = header + message + stackTrace;
                return description;
            }
        }

        #endregion Private data structures

        #region Private constants

        private const string DailySeparator = "****************************************************" +
            "**************************************************************************************";

        private const string EntrySeparator = "----------------------------------------------------" +
            "--------------------------------------------------------------------------------------";

        private const int EntryDescriptionLength = 104; // Header entry description length
        private const int LineTypeLength = 6;          // Header type length
        private const int LineTimestampLength = 23;    // Header timestamp length

        private const int UtfSignatureSize = 3; // In bytes

        #endregion Private constants

        #region Private variables

        private static Exception lastException = null;
        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static readonly object sync = new object();

        #endregion Private variables

        #region Public properties

        /// <summary>
        /// The minimum <see cref="Severity"/> level of the entry to log
        /// </summary>
        /// <remarks>
        /// Please note that the default minimum level
        /// is <see cref="Severity.Debug"/>
        /// </remarks>
        public static Severity MinimumSeverityLevel { get; private set; } = Severity.Debug;

        /// <summary>
        /// The log file path
        /// </summary>
        public static string Path { get; private set; } = "log.log";

        public static string ErrorsPath { get; private set; } = "errors.log";

        /// <summary>
        /// Define whether the <see cref="Logger"/> has been initialized by calling <see cref="Initialize(string, int)"/> -
        /// <see langword="true"/> - or not - <see langword="false"/>
        /// </summary>
        public static bool Initialized { get; private set; } = false;

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Initialize the logger with the specified parameters
        /// </summary>
        /// <remarks>
        /// If the <paramref name="daysOfLogsToKeepSaved"/> is equal to -1 no log file will be deleted (i.e. all the logs will be kept saved in the disk),
        /// otherwise logs older than the actual day minus the time span specified will be deleted (e.g. if today is 10/01/2021 and
        /// <paramref name="daysOfLogsToKeepSaved"/> is 10, then all the logs up to 30/12/2020 will be deleted)
        /// </remarks>
        /// <param name="logPath">The path of the log file</param>
        /// <param name="daysOfLogsToKeepSaved">The number of days of logs to keep saved up to now; -1 equals no file deleted</param>
        public static void Initialize(string logPath = "logs\\", int daysOfLogsToKeepSaved = -1)
        {
            DeleteOldLogs(logPath, daysOfLogsToKeepSaved);

            string errorsPath = System.IO.Path.Combine(logPath, "errors\\");
            DeleteOldLogs(errorsPath, daysOfLogsToKeepSaved);

            string now = DateTime.Now.ToString("yyyy-MM-dd");
            Path = logPath + $"{now}.log";
            IoUtility.CreateDirectoryIfNotExists(logPath);

            ErrorsPath = errorsPath + $"{now}.log";
            IoUtility.CreateDirectoryIfNotExists(errorsPath);

            InitializeFile(Path);
            InitializeFile(ErrorsPath);

            Initialized = true;
        }

        /// <summary>
        /// Set the <see cref="MinimumSeverityLevel"/> <see cref="Severity"/> to log (i.e. all the entry with a lower severity will not be logged)
        /// </summary>
        /// <remarks>
        /// Note that the minimum level of the logged entry can't be higher than <see cref="Severity.Info"/> (i.e. entry of level
        /// <see cref="Severity.Warn"/>, <see cref="Severity.Error"/> and <see cref="Severity.Fatal"/> will always be logged). <br/>
        /// The <see cref="Severity"/> level is defined as follows (from lower to higher): <see cref="Severity.Trace"/>,
        /// <see cref="Severity.Debug"/>, <see cref="Severity.Info"/>, <see cref="Severity.Warn"/>, <see cref="Severity.Error"/> and <see cref="Severity.Fatal"/>
        /// </remarks>
        /// <param name="level">The <see cref="Severity"/> level</param>
        public static void SetMinimumSeverityLevel(Severity level)
        {
            Severity oldSeverity = MinimumSeverityLevel;

            if ((int)level < (int)Severity.Warn)
            {
                MinimumSeverityLevel = level;
            }
            else
            {
                MinimumSeverityLevel = Severity.Warn;
            }

            if (oldSeverity != MinimumSeverityLevel)
            {
                Warn($"Minimum level set from {GetReadableSeverityAsString(oldSeverity)} to {GetReadableSeverityAsString(MinimumSeverityLevel)}.");
            }
        }

        #region Synchronous logging methods

        /// <summary>
        /// Simple log method.
        /// Save the text specified as parameter in the log file.
        /// The file will be saved based on what is specified inside <see cref="Path"/>
        /// </summary>
        /// <param name="text">The text to be saved</param>
        /// <param name="severity">The <see cref="Severity"/></param>
        public static void Log(string text, Severity severity = Severity.Info)
        {
            if (HasHigherSeverityLevel(severity))
            {
                string log = BuildLogEntry(text, severity);
                AppendText(log, Path);

                if (severity == Severity.Error || severity == Severity.Fatal)
                {
                    AppendText(log, ErrorsPath);
                }
            }
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
            bool alreadyLogged = HasExceptionAlreadyBeenLogged(ex);

            if (!alreadyLogged || !IsSameExceptionAsTheLast(ex))
            {
                ExceptionEntry entry = BuildLogEntry(ex);

                AppendText(entry, Path);
                AppendText(entry, ErrorsPath);
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
        /// Save the text specified as <see cref="Severity.Debug"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Debug(string text)
            => Log(text, Severity.Debug);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Info"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Info(string text)
            => Log(text, Severity.Info);

        /// <summary>
        /// Save the text specified as <see cref="Severity.Warn"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        public static void Warn(string text)
            => Log(text, Severity.Warn);

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
        /// Save the text specified as <see cref="Severity.Fatal"/>
        /// in the log file. <br/>
        /// See also <see cref="Log(string, Severity)"/>
        /// </summary>
        /// <remarks>If the log is required after an <see cref="Exception"/>
        /// occurred, consider using the method <see cref="Log(Exception)"/> instead!</remarks>
        /// <param name="text">The text to log</param>
        public static void Fatal(string text)
            => Log(text, Severity.Fatal);

        #endregion Synchronous logging methods

        #region Asynchronous logging methods

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
                await AppendTextAsync(log, Path, hasToWait: true);

                if (severity == Severity.Error || severity == Severity.Fatal)
                {
                    await AppendTextAsync(log, ErrorsPath, hasToWait: true);
                }
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
            bool alreadyLogged = HasExceptionAlreadyBeenLogged(ex);

            if (!alreadyLogged || !IsSameExceptionAsTheLast(ex))
            {
                ExceptionEntry entry = BuildLogEntry(ex);

                await AppendTextAsync(entry, Path);
                await AppendTextAsync(entry, ErrorsPath);
            }
        }

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
        /// Save asynchronously the text specified as <see cref="Severity.Debug"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task DebugAsync(string text)
            => await LogAsync(text, Severity.Debug);

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
        /// Save asynchronously the text specified as <see cref="Severity.Warn"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task WarnAsync(string text)
            => await LogAsync(text, Severity.Warn);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Error"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task ErrorAsync(string text)
            => await LogAsync(text, Severity.Error);

        /// <summary>
        /// Save asynchronously the <see cref="Exception"/> to the log file.
        /// See also <see cref="LogAsync(Exception)"/>
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> occurred</param>
        public static async Task ErrorAsync(Exception ex)
            => await LogAsync(ex);

        /// <summary>
        /// Save asynchronously the text specified as <see cref="Severity.Fatal"/>
        /// in the log file. <br/>
        /// See also <see cref="LogAsync(string, Severity)"/>
        /// </summary>
        /// <param name="text">The text to log</param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task FatalAsync(string text)
            => await LogAsync(text, Severity.Fatal);

        #endregion Asynchronous logging methods

        #endregion Public methods

        #region Helper methods

        /// <summary>
        /// Initialize the file on <see cref="Initialize(string, int)"/>
        /// </summary>
        /// <param name="path">The path of the log file</param>
        private static void InitializeFile(string path)
        {
            // If file doesn't exist or it exists but has a size of 3B or less print headers
            // 3B are the UTF-8 signature and are always present in an UTF-8 encoded file
            if (!File.Exists(path) || (File.Exists(path) && new FileInfo(path).Length <= UtfSignatureSize))
            {
                string header = $"UTC time: {GetUtcDateTime()}, current zone time ({TimeZone.CurrentTimeZone.StandardName}): {GetDateTime()}{Environment.NewLine}" +
                    $"User name: {Environment.UserName}{Environment.NewLine}" +
                    $"User domain name: {Environment.UserDomainName}{Environment.NewLine}" +
                    $"Application friendly name: {AppDomain.CurrentDomain.FriendlyName}{Environment.NewLine}" +
                    $"# Application additional information #{Environment.NewLine}" +
                    $"\tApplication id: {AppDomain.CurrentDomain.Id}{Environment.NewLine}" +
                    $"\tApplication base directory: {AppDomain.CurrentDomain.BaseDirectory}{Environment.NewLine}" +
                    $"\tApplication current directory: {Environment.CurrentDirectory}{Environment.NewLine}" +
                    $"\tApplication command line: {Environment.CommandLine}{Environment.NewLine}" +
                    $"\tApplication 64-bit: {Environment.Is64BitProcess}{Environment.NewLine}" +
                    $"# OS additional information #{Environment.NewLine}" +
                    $"\tOS version: {Environment.OSVersion}{Environment.NewLine}" +
                    $"\tOS 64-bit: {Environment.Is64BitOperatingSystem}{Environment.NewLine}" +
                    $"# Machine additional information #{Environment.NewLine}" +
                    $"\tMachine processor count: {Environment.ProcessorCount}{Environment.NewLine}" +
                    $"\tMachine processor architecture: {Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE")}{Environment.NewLine}" +
                    $"\tMachine processor model: {Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER")}{Environment.NewLine}" +
                    $"\tMachine processor level: {Environment.GetEnvironmentVariable("PROCESSOR_LEVEL")}{Environment.NewLine}" +
                    $"\tCLR version: {Environment.Version}{Environment.NewLine}";

                StringBuilder systemInfo = new StringBuilder();
                systemInfo.AppendFormat($"\tLogical drives:{Environment.NewLine}");
                foreach (DriveInfo driveInfo in DriveInfo.GetDrives())
                {
                    try
                    {
                        systemInfo.AppendFormat(
                            "\t\t- Drive: {0}\n" +
                            "\t\t\tVolume label: {1}\n" +
                            "\t\t\tDrive type: {2}\n" +
                            "\t\t\tDrive format: {3}\n" +
                            "\t\t\tTotal size [GB]: {4}\n" +
                            "\t\t\tAvailable free space [GB]: {5}\n",
                            driveInfo.Name,
                            driveInfo.VolumeLabel,
                            driveInfo.DriveType,
                            driveInfo.DriveFormat,
                            driveInfo.TotalSize / 1024 / 1024 / 1024, // From B to GB
                            driveInfo.AvailableFreeSpace / 1024 / 1024 / 1024 // From B to GB
                        );
                    }
                    catch (Exception ex) // This should not throw any Exception, just for precaution
                    {
                        systemInfo.AppendLine(ex.Message);
                    }
                }
                header += systemInfo.ToString();

                AppendText(header, path);
                AppendText(DailySeparator, path);

                string lineTimestamp = "", lineType = "", lineLogEntryDescription = "";
                for (int i = 0; i <= EntryDescriptionLength; i++)
                {
                    if (i <= LineTypeLength)
                    {
                        lineType += "*";
                    }

                    if (i <= LineTimestampLength)
                    {
                        lineTimestamp += "*";
                    }

                    lineLogEntryDescription += "*";
                }

                // 23 = LineTimestampLength
                // 5  = LineTypeLength - 1
                // 70 = EntryDescriptionLength
                header = string.Format("{0, 23}|{1, 5}|{2, 70}", lineTimestamp, lineType, lineLogEntryDescription);
                header += Environment.NewLine;
                header += string.Format("{0, 23} | {1, 5} | {2, 56}", "TIMESTAMP", "TYPE", "LOG ENTRY DESCRIPTION");
                header += Environment.NewLine;
                header += string.Format("{0, 23}|{1, 5}|{2, 70}", lineTimestamp, lineType, lineLogEntryDescription);

                AppendText(header, path);
            }
            else
            {
                AppendText(DailySeparator, path);
            }
        }

        /// <summary>
        /// Delete the old logs if necessary
        /// </summary>
        /// <param name="logPath">The log file path</param>
        /// <param name="timeSpanAsDays">The time span of file that has to be keep saved (in days)</param>
        private static void DeleteOldLogs(string logPath, int timeSpanAsDays)
        {
            string[] files;
            if (Directory.Exists(logPath))
            {
                files = Directory.GetFiles(logPath);
            }
            else
            {
                Directory.CreateDirectory(logPath);
                files = Directory.GetFiles(logPath);
            }

            if (timeSpanAsDays != -1)
            {
                DeleteLogs(timeSpanAsDays, files);
            }
        }

        /// <summary>
        /// Deletes old log files saved in disk after they have been retrieved (see <see cref="DeleteOldLogs(string, int)"/>
        /// </summary>
        /// <param name="timeSpanAsDays">The time span of file that has to be keep saved (in days)</param>
        /// <param name="files">The <see cref="string"/> array containing the log files path</param>
        private static void DeleteLogs(int timeSpanAsDays, string[] files)
        {
            foreach (string file in files)
            {
                string extension = new FileInfo(file).Extension;

                string[] str = file.Split('-');
                int year = int.Parse(str[0].Replace("logs\\", string.Empty).Replace("errors\\", string.Empty)); // Remove initial folders
                int month = int.Parse(str[1]);
                int day = int.Parse(str[2].Replace(extension, string.Empty));

                DateTime date = new DateTime(year, month, day);
                int days = DateTime.Now.Subtract(date).Days;

                if (days > timeSpanAsDays)
                {
                    File.Delete(file);
                }
            }
        }

        /// <summary>
        /// Create an entry as a <see cref="Tuple"/>
        /// </summary>
        /// <param name="severity">The <see cref="Severity"/></param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <param name="stackTrace">The <see cref="StackTrace"/> (as <see cref="string"/>)</param>
        /// <returns>The <see cref="ExceptionEntry"/> containing the entry</returns>
        private static ExceptionEntry CreateEntry(Severity severity, string source, string message, string stackTrace)
        {
            string severityAsString = GetSeverityAsString(severity);
            string now = GetDateTime();

            ExceptionEntry entry = new ExceptionEntry
            {
                Timestamp = now,
                Severity = severityAsString,
                Source = source,
                Message = message,
                StackTrace = stackTrace
            };

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
                {
                    line += "-"; // Normal line separator
                }
                else
                {
                    line += "|"; // Add this char in these position for a table-like appearance
                }
            }

            log += Environment.NewLine + line;

            return log;
        }

        /// <summary>
        /// Build a new log entry
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to log</param>
        /// <returns>A <see cref="ExceptionEntry"/> containing the new entry</returns>
        private static ExceptionEntry BuildLogEntry(Exception ex)
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

            ExceptionEntry entry = CreateEntry(
                Severity.Error,
                $"{source} - {type} on line {line} (method: {methodName}, file: {fileName})",
                message,
                stackTrace
            );

            return entry;
        }

        /// <summary>
        /// Check if an <see cref="Exception"/> has already been logged
        /// </summary>
        /// <param name="ex">The <see cref="Exception"/> to check</param>
        /// <returns><see langword="false"/> if an <see cref="Exception"/> has not yet been logged, <see langword="true"/> otherwise</returns>
        private static bool HasExceptionAlreadyBeenLogged(Exception ex)
        {
            bool logged;

            if (lastException == null) // First exception thrown
            {
                lastException = ex; // lastException == ex -> true
                logged = false; // force the next if to be true: IsSameExceptionAsTheLast -> true, flag -> true
            }
            else
            {
                logged = lastException != null; // If lastException != null -> true
            }

            return logged;
        }

        /// <summary>
        /// Append text on the log file. See <see cref="FileHandler.Save(string, string, SaveMode)"/>.
        /// </summary>
        /// <param name="text">The text to append</param>
        private static void AppendText(string text, string path)
        {
            lock (sync)
                Save(text, path, SaveMode.Append);
        }

        /// <summary>
        /// Append text on the log file asynchronously. See <see cref="FileHandler.SaveAsync(string, string, SaveMode)"/>
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="hasToWait"><see langword="true"/> if the task has to wait for a semaphore, <see langword="false"/> otherwise</param>
        /// <returns>The async <see cref="Task"/></returns>
        private static async Task AppendTextAsync(string text, string path, bool hasToWait = true)
        {
            if (hasToWait)
            {
                await semaphore.WaitAsync();
            }

            await SaveAsync(text, path, SaveMode.Append);

            if (hasToWait)
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Append an <see cref="ExceptionEntry"/> to the log file
        /// </summary>
        /// <param name="entry">The <see cref="ExceptionEntry"/> containing the element to append</param>
        private static void AppendText(ExceptionEntry entry, string path)
        {
            AppendText(entry.ToString(), path);
            AppendText(EntrySeparator + Environment.NewLine, path);
        }

        /// <summary>
        /// Append asynchronously an <see cref="ExceptionEntry"/> to the log file
        /// </summary>
        /// <param name="entry">The <see cref="ExceptionEntry"/> containing the element to append</param>
        /// <returns>The async <see cref="Task"/></returns>
        private static async Task AppendTextAsync(ExceptionEntry entry, string path)
        {
            // Here, hasToAwait has been set to false because the method enter the semaphore once and then release it
            // at the end of all the operations. So, there's no need to await another time inside the AppendTextAsync method!
            await semaphore.WaitAsync();

            await AppendTextAsync(entry.ToString(), path, hasToWait: false);
            await AppendTextAsync(EntrySeparator + Environment.NewLine, path, hasToWait: false);

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
        /// Convert the <see cref="Severity"/> of the entry to log in a readable <see cref="string"/> (i.e. removing all unnecessary characters as white spaces)
        /// </summary>
        /// <param name="severity"> The severity (<see cref="Severity"/>) of the entry </param>
        /// <returns>The <see cref="string"/> result of the conversion</returns>
        private static string GetReadableSeverityAsString(Severity severity)
        {
            string str = GetSeverityAsString(severity);
            str = new string(str.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());

            return str;
        }

        /// <summary>
        /// Get the <see cref="DateTime"/> in the format "yyyy/MM/dd-HH:mm:ss:fff". <br/>
        /// For example: 2021/02/25-09:32:44:123.
        /// </summary>
        /// <returns>The <see cref="string"/> representing the <see cref="DateTime"/></returns>
        private static string GetDateTime()
             => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff");

        /// <summary>
        /// Get the UTC <see cref="DateTime"/> in the format "yyyy/MM/dd-HH:mm:ss:fff". <br/>
        /// For example: 2021/02/25-09:32:44:123.
        /// </summary>
        /// <returns>The <see cref="string"/> representing the <see cref="DateTime"/></returns>
        private static string GetUtcDateTime()
             => DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.fff");

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
        /// <returns><see langword="true"/> if is the new <see cref="Exception"/> is equal to the last one, <see langword="false"/> otherwise</returns>
        private static bool IsSameExceptionAsTheLast(Exception ex)
        {
            bool isSameException = ex.GetType() == lastException.GetType();
            isSameException &= ex.Message.CompareTo(lastException.Message) == 0;
            isSameException &= ex.StackTrace == lastException.StackTrace;

            return isSameException;
        }

        /// <summary>
        /// Test if the entry to log has an higher (or equal) <see cref="Severity"/> level than the <see cref="MinimumSeverityLevel"/> set
        /// </summary>
        /// <param name="level">The <see cref="Severity"/> level to test</param>
        /// <returns> <see langword="true"/> if the level to test is higher (or equals) to <see cref="MinimumSeverityLevel"/>, <see langword="false"/> otherwise </returns>
        private static bool HasHigherSeverityLevel(Severity level)
        {
            bool isHigher = (int)MinimumSeverityLevel <= (int)level;
            return isHigher;
        }

        #endregion Helper methods
    }
}