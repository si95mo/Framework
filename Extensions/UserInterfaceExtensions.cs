using System;
using System.Windows.Forms;
using UserInterface.Dashboards;

namespace Extensions
{
    public static class UserInterfaceExtensions
    {
        /// <summary>
        /// Automatically update a <see cref="Control"/> if needed (e.g. in case of a cross-thread operation)
        /// </summary>
        /// <param name="source">The source <see cref="Control"/></param>
        /// <param name="form">The parent <see cref="Form"/></param>
        /// <param name="updateAction">The <see cref="Action"/> to call in case of a non cross-thread operation</param>
        /// <param name="crossThreadMethod">The cross-thread <see cref="Delegate"/> method (e.g. a value changed handler)</param>
        /// <param name="args">The <paramref name="crossThreadMethod"/> arguments</param>
        public static void InvokeIfNeeded(this Control source, Form form, Action updateAction, Delegate crossThreadMethod, params object[] args)
        {
            if (!form.InvokeRequired)
                updateAction();
            else
                source.BeginInvoke(crossThreadMethod, args);
        }

        /// <summary>
        /// Set the <see cref="DraggableControl"/> draggable option
        /// </summary>
        /// <param name="source">The source <see cref="Control"/></param>
        /// <param name="isDraggable">
        /// <see langword="true"/> if the <see cref="DraggableControl"/> will be draggable,
        /// <see langword="false"/> otherwise
        /// </param>
        public static void SetDraggable(this DraggableControl source, bool isDraggable)
        {
            source.Draggable(isDraggable);
        }
    }
}