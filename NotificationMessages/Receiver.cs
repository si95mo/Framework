using Diagnostic;
using System;
using System.IO.MemoryMappedFiles;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NotificationMessages
{
    /// <summary>
    /// Implement a notification receiver
    /// </summary>
    public class Receiver : IDisposable
    {
        /// <summary>
        /// The shared file path. See <see cref="Sender.SharedFilePath"/>
        /// </summary>
        public string SharedFilePath { get; set; } = string.Empty;

        private MemoryMappedFile memoryMappedFile;
        private bool disposed = false;

        /// <summary>
        /// Start the receiver
        /// </summary>
        public void Start()
        {
            uint mainThreadId = GetCurrentThreadId();

            memoryMappedFile = MemoryMappedFile.CreateNew(SharedFilePath, IntPtr.Size, MemoryMappedFileAccess.ReadWrite);
            using (MemoryMappedViewAccessor accessor = memoryMappedFile.CreateViewAccessor(0, IntPtr.Size, MemoryMappedFileAccess.ReadWrite))
                accessor.Write(0, mainThreadId);

            Application.AddMessageFilter(new MessageFilter());
            Logger.Info($"Receiver started with main thread id {mainThreadId}");

            Application.Run();
        }

        [DllImport("kernel32.dll")]
        private static extern uint GetCurrentThreadId();

        public void Dispose()
            => memoryMappedFile.Dispose();
    }
}