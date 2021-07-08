using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Diagnostic
{
    /// <summary>
    /// Class the continuously read a log file.
    /// See also <see cref="Logger"/>
    /// </summary>
    public class LogReader
    {
        private static string logText = "";
        private static string lastLog = "";
        private static bool reading = false;

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
        { get; private set; }

        /// <summary>
        /// Define whether the log file is being read (<see langword="true"/>)
        /// or not (<see langword="false"/>)
        /// </summary>
        public static bool Reading => reading;

        /// <summary>
        /// Start the read of the log file.
        /// </summary>
        /// <param name="logPath">The log file path</param>
        /// <remarks>The <see cref="Logger"/> must be <see cref="Logger.Initialized"/></remarks>
        public static void StartReading(string logPath)
        {
            if (Logger.Initialized)
            {
                LogPath = logPath;

                if (!reading)
                {
                    StartMonitoring().Start();
                    reading = true;
                }
            }
        }

        /// <summary>
        /// Create the log file monitoring <see cref="Task"/>
        /// </summary>
        /// <returns>The monitoring <see cref="Task"/></returns>
        private static Task StartMonitoring()
        {
            long initialFileSize = new FileInfo(LogPath).Length;
            long lastReadLength = initialFileSize - 1024;

            if (lastReadLength < 0)
                lastReadLength = 0;

            Task monitoringTask = new Task(() =>
                {
                    while (true)
                    {
                        try
                        {
                            var fileSize = new FileInfo(LogPath).Length;
                            if (fileSize > lastReadLength)
                            {
                                using (var fs = new FileStream(LogPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
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
                            reading = false;
                        }

                        Thread.Sleep(1000);
                    }
                }
            );

            return monitoringTask;
        }
    }
}
