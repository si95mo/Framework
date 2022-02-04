using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement useful extension methods for <see cref="UserControl"/> objects
    /// </summary>
    public static class UserControlExtensions
    {
        /// <summary>
        /// Perform an update on the <see cref="UserControl"/> from a different
        /// thread than the one in which <paramref name="source"/> has been created
        /// </summary>
        /// <param name="source">The source control to update</param>
        /// <param name="action">The update <see cref="Action"/></param>
        public static void Update(this UserControl source, Action action)
            => source.Invoke(new MethodInvoker(() => action()));
    }
}