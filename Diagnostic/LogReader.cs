using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Diagnostic
{
    /// <summary>
    /// Class the continuously read a log file.
    /// See also <see cref="Logger"/>
    /// </summary>
    public class LogReader
    {
        private static string logPath = "";
        private static string logText = "";
        private static string lastLog = "";
        private static bool reading = false;
        private static int monitoringInterval = 1000; // ms
        private static Task monitoringTask;

        /// <summary>
        /// The full log text with all the entries
        /// </summary>
        public static string LogText => logText;

        /// <summary>
        /// The last log entry
        /// </summary>
        public static string LastLog => lastLog;

        /// <summary>
        /// The log file path
        /// </summary>
        public static string LogPath
        {
            get => logPath;
            private set => logPath = value;
        }

        /// <summary>
        /// Define whether the log file is being read (<see langword="true"/>)
        /// or not (<see langword="false"/>)
        /// </summary>
        public static bool Reading => reading;

        /// <summary>
        /// The monitoring interval in milliseconds
        /// </summary>
        public static int MonitoringInterval => monitoringInterval;

        /// <summary>
        /// Start the read of the log file.
        /// </summary>
        /// <param name="pollingInterval">The monitoring interval (in milliseconds)</param>
        /// <remarks>The <see cref="Logger"/> must be <see cref="Logger.Initialized"/></remarks>
        public static void StartRead(int pollingInterval = 1000)
        {
            if (Logger.Initialized)
            {
                logPath = Logger.Path;
                monitoringInterval = pollingInterval;

                if (!reading)
                {
                    monitoringTask = CreateMonitoringTask();
                    monitoringTask.Start();

                    reading = true;
                }
            }
        }

        /// <summary>
        /// Create the log file monitoring <see cref="Task"/>
        /// </summary>
        /// <returns>The monitoring <see cref="Task"/></returns>
        private static Task CreateMonitoringTask()
        {
            long initialFileSize = 0;
            long lastReadLength = initialFileSize - 1024;

            if (lastReadLength < 0)
                lastReadLength = 0;

            Task monitoringTask = new Task(async () =>
                {
                    while (true)
                    {
                        try
                        {
                            var fileSize = new FileInfo(logPath).Length;
                            if (fileSize > lastReadLength)
                            {
                                using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                                {
                                    fs.Seek(lastReadLength, SeekOrigin.Begin);
                                    var buffer = new byte[1024];

                                    while (true)
                                    {
                                        var bytesRead = fs.Read(buffer, 0, buffer.Length);
                                        lastReadLength += bytesRead;

                                        if (bytesRead == 0)
                                            break;

                                        var text = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                                        logText += text;
                                        lastLog = text;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(ex);
                        }

                        await Task.Delay(monitoringInterval);
                    }
                }
            );

            return monitoringTask;
        }
    }
}