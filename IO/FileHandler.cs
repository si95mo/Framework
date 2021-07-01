using System;
using System.IO;
using System.Linq;

/// <summary>
/// Static class that handles IO operations.
/// <see cref="IO.IOUtility"/>
/// </summary>
namespace IO
{
    /// <summary>
    /// Define the write mode (overwrite or append).
    /// </summary>
    public class FileHandler
    {
        public enum MODE
        {
            /// <summary>
            /// Specify the overwrite mode for writing
            /// </summary>
            Overwrite = 0,

            /// <summary>
            /// Specify the append mode for writing
            /// </summary>
            Append = 1
        };

        protected static StreamWriter sw;
        protected static StreamReader sr;

        /// <summary>
        /// Save text to a file.
        /// </summary>
        /// <param name="text">The text to be saved</param>
        /// <param name="path">Path to the file</param>
        /// <param name="mode">Write mode, overwrite or append <see cref="MODE"/></param>
        public static void Save(string text, string path, MODE mode = MODE.Overwrite)
        {
            try
            {
                switch (mode)
                {
                    case MODE.Overwrite:
                        using (sw = File.CreateText(path))
                        {
                            sw.WriteLine(text);
                        }
                        break;

                    case MODE.Append:
                        using (sw = File.AppendText(path))
                        {
                            sw.WriteLine(text);
                        }
                        break;
                }

                sw.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Read text from a file.
        /// </summary>
        /// <param name="path">The path to the file that contains the text to be read</param>
        /// <returns></returns>
        public static string Read(string path)
        {
            string linesRead = "";
            sr = new StreamReader(path);

            while (!sr.EndOfStream)
            {
                linesRead += sr.ReadLine() + Environment.NewLine;
            }

            sr.Close();

            return linesRead;
        }

        /// <summary>
        /// Copy a file.
        /// </summary>
        /// <param name="file">The path of the file to be copied</param>
        /// <param name="newFile">The path in where copy the file</param>
        public static void CopyFile(string file, string newFile)
        {
            File.Copy(file, newFile, true);
        }

        /// <summary>
        /// Copy all files in a directory, overwriting existing modified files.
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="newDirectoryPath">The new directory path</param>
        public static void CopyAllFilesInDirectory(string directoryPath, string newDirectoryPath)
        {
            string[] files = Directory.GetFiles(directoryPath);
            string newFile;

            foreach (string file in files)
            {
                newFile = "" + newDirectoryPath + "\\" + Path.GetFileName(file);

                if (IsFileModified(file, newFile))
                    File.Copy(file, newFile, true);
            }
        }

        /// <summary>
        /// Copy all files in a directory with specified extensions,
        /// overwriting existing modified files.
        /// </summary>
        /// <param name="directoryPath">The directory path</param>
        /// <param name="newDirectoryPath">The new directory path</param>
        /// <param name="extensions">An array of string containing the extensions
        /// of the file to be copied (e.g. ".pdf")</param>
        public static void CopyAllFilesInDirectory(string directoryPath, string newDirectoryPath, string[] extensions)
        {
            bool doCopy = false;
            string[] files = Directory.GetFiles(directoryPath);
            string newFile;
            string extension;

            foreach (string file in files)
            {
                newFile = "" + newDirectoryPath + "\\" + Path.GetFileName(file);
                extension = Path.GetExtension(file);

                for (int i = 0; i < extensions.Count() && !doCopy; i++)
                    doCopy = doCopy || extension.CompareTo(extensions[i]) == 0;

                if (doCopy)
                {
                    if (IsFileModified(file, newFile))
                        File.Copy(file, newFile, true);

                    doCopy = false;
                }
            }
        }

        /// <summary>
        /// Check if a file has been modified (i.e. last time it was written).
        /// </summary>
        /// <param name="existingFIle">The old file path present in disk</param>
        /// <param name="fileToPaste">The new file path to check if modified</param>
        /// <returns><see langword="true"/> if <paramref name="fileToPaste"/> has been modified
        /// and its different from <paramref name="existingFIle"/>,
        /// <see langword="false"/>  otherwise</returns>
        private static bool IsFileModified(string existingFIle, string fileToPaste)
        {
            DateTime existingFileLastWrittenTime = File.GetLastWriteTime(existingFIle);
            DateTime fileToPasteLastWrittenTime = File.GetLastWriteTime(fileToPaste);

            double difference = Math.Abs(existingFileLastWrittenTime.Subtract(fileToPasteLastWrittenTime).TotalSeconds);

            bool isModified = difference != 0;

            return isModified;
        }
    }
}