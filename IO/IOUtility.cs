using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace IO
{
    /// <summary>
    /// Static class that adds extra functions to IO operation.
    /// <see cref="FileHandler"/>
    /// </summary>
    public class IOUtility
    {
        /// <summary>
        /// Get to desktop folder.
        /// </summary>
        /// <returns> Path to the desktop folder </returns>
        public static string GetDesktopFolder()
        {
            string str = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            return str;
        }

        /// <summary>
        /// Check if a directory exists.
        /// </summary>
        /// <param name="path"> The path to the folder to check </param>
        /// <returns> <see langword="true"/> if the directory exists, <see langword="false"/> otherwise </returns>
        public static bool DoesDirectoryExists(string path)
        {
            bool doesExists = Directory.Exists(path);

            return doesExists;
        }

        /// <summary>
        /// Check if a file exists.
        /// </summary>
        /// <param name="path"> The path to the file to check </param>
        /// <returns> <see langword="true"/>  if the file exists, <b>false</b> otherwise </returns>
        public static bool DoesFileExists(string path)
        {
            bool doesExists = File.Exists(path);

            return doesExists;
        }

        /// <summary>
        /// Create a directory if not exists at the path specified.
        /// </summary>
        /// <param name="path"> The path where to create the new folder </param>
        /// <returns> <see langword="true"/>  if the directory has been created, <b>false</b> otherwise</returns>
        public static bool CreateDirectoryIfNotExists(string path)
        {
            bool directoryCreated = false;

            if (!DoesDirectoryExists(path))
            {
                Directory.CreateDirectory(path);
                directoryCreated = true;
            }

            return directoryCreated;
        }

        /// <summary>
        /// Extract the icon from all files contained in the folder specified
        /// </summary>
        /// <param name="path"> The folder path </param>
        /// <returns> A <see cref="List{T}"/> of <see cref="FileItem"/>
        /// containing the name and the icon of each file </returns>
        public static List<FileItem> GetFileIcon(string path)
        {
            List<FileItem> items = new List<FileItem>(); // List of entries

            // Extract the icon from each file in the folder
            foreach (var file in Directory.GetFiles(path))
            {
                Icon icon = Icon.ExtractAssociatedIcon(file);
                items.Add(new FileItem { Key = Path.GetFileName(file), Value = icon.ToBitmap() });
            }

            return items;
        }
    }
}