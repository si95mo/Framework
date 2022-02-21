using System;
using System.Windows.Forms;

namespace Extensions
{
    public static class UserInterfaceExtensions
    {
        /// <summary>
        /// Automatically update an <see cref="UserControl"/> id neede (e.g. in case of a cross-thread operation)
        /// </summary>
        /// <param name="source">The source <see cref="UserControl"/></param>
        /// <param name="form">The parent <see cref="Form"/></param>
        /// <param name="updateAction">The <see cref="Action"/> to call in case of a non cross-thread operation</param>
        /// <param name="crossThreadMethod">The cross-thread <see cref="Delegate"/> method (e.g. a value changed handler)</param>
        /// <param name="args">The <paramref name="crossThreadMethod"/> arguments</param>
        public static void InvokeIfNeeded(this UserControl source, Form form, Action updateAction, Delegate crossThreadMethod, params object[] args)
        {
            if (!form.InvokeRequired)
                updateAction();
            else
                source.BeginInvoke(crossThreadMethod, args);
        }
    }
}
