using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    internal static class ExtensionMethods
    {
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
            source.IsDraggable = isDraggable;
            source.Draggable(isDraggable);
        }
    }
}
