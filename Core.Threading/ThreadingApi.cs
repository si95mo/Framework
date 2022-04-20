using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;

namespace Core.Threading
{
    /// <summary>
    /// Provide thread-related API
    /// </summary>
    internal class ThreadingApi
    {
        /// <summary>
        /// Set the internal interrupt clock rate. <br/>
        /// See the Windows API documentation for details.
        /// </summary>
        /// <param name="uMilliseconds">The new clock rate (in milliseconds)</param>
        /// <returns>The interval</returns>
        /// <remarks>
        /// This call is not permanent as the clock rate will be set to its default
        /// value on system start up!
        /// </remarks>
        [
            SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
            SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"),
            SuppressUnmanagedCodeSecurity
        ]
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod", SetLastError = true)]
        internal static extern uint SetTimeBeginPeriod(uint uMilliseconds);

        /// <summary>
        /// Reset the internal interrupt clock rate (the one specified in. <br/>
        /// See the Windows API documentation for details.
        /// </summary>
        /// <param name="uMilliseconds">The clock rate (in milliseconds) to stop</param>
        /// <returns>The interval</returns>
        /// <remarks>
        /// This call is not permanent as the clock rate will be set to its default
        /// value on system start up!
        /// </remarks>
        [
            SuppressMessage("Microsoft.Interoperability", "CA1401:PInvokesShouldNotBeVisible"),
            SuppressMessage("Microsoft.Security", "CA2118:ReviewSuppressUnmanagedCodeSecurityUsage"),
            SuppressUnmanagedCodeSecurity
        ]
        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod", SetLastError = true)]
        internal static extern uint SetTimeEndPeriod(uint uMilliseconds);
    }
}