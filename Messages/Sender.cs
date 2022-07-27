using System;
using System.Diagnostics;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Runtime.InteropServices;

namespace Messages
{
    /// <summary>
    /// Implement a notification sender
    /// </summary>
    public class Sender
    {
        /// <summary>
        /// The shared file path. See <see cref="Receiver.SharedFilePath"/>
        /// </summary>
        public string SharedFilePath { get; set; } = string.Empty;

        public void Start()
        {
            string processName = "Receiver";
            Process[] processes = Process.GetProcesses();

            Process process = (from p in processes where p.ProcessName.ToUpper().Contains(processName) select p).First();

            uint threadId;
            using (var mmf = MemoryMappedFile.OpenExisting(SharedFilePath, MemoryMappedFileRights.Read))
            using (var accessor = mmf.CreateViewAccessor(0, IntPtr.Size, MemoryMappedFileAccess.Read))
                accessor.Read(0, out threadId);

            PostThreadMessage(threadId, (uint)MessageType.User, IntPtr.Zero, IntPtr.Zero);
        }

        [return: MarshalAs(UnmanagedType.Bool)]
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool PostThreadMessage(uint threadId, uint msg, IntPtr wParam, IntPtr lParam);
    }
}