using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IO
{
    /// <summary>
    /// Define the write mode (overwrite or append).
    /// </summary>
    public class FileHandler
    {
        /// <summary>
        /// Define the save mode
        /// </summary>
        public enum SaveMode
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

        private static object writeLock = new object();

        /// <summary>
        /// The <see cref="StreamWriter"/>
        /// </summary>
        protected static StreamWriter sw;

        /// <summary>
        /// The <see cref="StreamReader"/>
        /// </summary>
        protected static StreamReader sr;

        /// <summary>
        /// Save text to a file.
        /// </summary>
        /// <param name="text">The text to save</param>
        /// <param name="path">Path to the file</param>
        /// <param name="mode">Write mode, overwrite or append <see cref="SaveMode"/></param>
        public static void Save(string text, string path, SaveMode mode = SaveMode.Overwrite)
        {
            lock (writeLock)
            {
                switch (mode)
                {
                    case SaveMode.Overwrite:
                        using (sw = new StreamWriter(path, false, Encoding.UTF8))
                            sw.WriteLine(text);
                        break;

                    case SaveMode.Append:
                        using (sw = new StreamWriter(path, true, Encoding.UTF8))
                            sw.WriteLine(text);
                        break;
                }

                sw.Close();
            }
        }

        /// <summary>
        /// Save text to a file asynchronously
        /// </summary>
        /// <param name="text">The text to save</param>
        /// <param name="path">Path to the file</param>
        /// <param name="mode">Write mode, overwrite or append <see cref="SaveMode"/></param>
        /// <returns>The async <see cref="Task"/></returns>
        public static async Task SaveAsync(string text, string path, SaveMode mode = SaveMode.Overwrite)
        {
            byte[] encodedText = Encoding.UTF8.GetBytes(text);
            FileMode fileMode = mode == SaveMode.Overwrite ? FileMode.Create : FileMode.Append;

            using (FileStream stream = new FileStream(path, fileMode, FileAccess.Write, FileShare.ReadWrite, 4096, true))
            {
                await stream.WriteAsync(encodedText, 0, encodedText.Length);
            }
        }

        /// <summary>
        /// Read text from a file.
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The text read</returns>
        public static string Read(string path)
        {
            string linesRead = "";
            sr = new StreamReader(path, Encoding.UTF8);

            while (!sr.EndOfStream)
                linesRead += sr.ReadLine() + Environment.NewLine;

            sr.Close();

            return linesRead;
        }

        /// <summary>
        /// Read text from a file asynchronously
        /// </summary>
        /// <param name="path">The file path</param>
        /// <returns>The (async) <see cref="Task"/> of which the result is the text read</returns>
        public static async Task<string> ReadAsync(string path)
        {
            byte[] buffer;
            using (FileStream stream = new FileStream(path, FileMode.Open))
            {
                buffer = new byte[stream.Length];
                await stream.ReadAsync(buffer, 0, (int)stream.Length);
            }

            string text = Encoding.UTF8.GetString(buffer);
            return text;
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
        /// <param name="existingFile">The old file path present in disk</param>
        /// <param name="fileToPaste">The new file path to check if modified</param>
        /// <returns><see langword="true"/> if <paramref name="fileToPaste"/> has been modified
        /// and its different from <paramref name="existingFile"/>,
        /// <see langword="false"/> otherwise</returns>
        private static bool IsFileModified(string existingFile, string fileToPaste)
        {
            DateTime existingFileLastWrittenTime = File.GetLastWriteTime(existingFile);
            DateTime fileToPasteLastWrittenTime = File.GetLastWriteTime(fileToPaste);

            double difference = Math.Abs(existingFileLastWrittenTime.Subtract(fileToPasteLastWrittenTime).TotalSeconds);

            bool isModified = difference != 0;

            return isModified;
        }
    }
}