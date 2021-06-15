using System.Drawing;

namespace IO
{
    /// <summary>
    /// Represent a file with a <see cref="Key"/> and a <see cref="Value"/>
    /// (where Key = file name and Value = file icon).
    /// </summary>
    public class FileItem
    {
        /// <summary>
        /// Key feature, file name
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// value feature, file icon
        /// </summary>
        public Image Value { get; set; }
    }
}
