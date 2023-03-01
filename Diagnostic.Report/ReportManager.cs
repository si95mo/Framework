using IO;
using System;
using System.IO;
using System.Threading.Tasks;
using static IO.FileHandler;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a generic report manager (i.e. log a measure or a message that is not log-related to a file). See <see cref="IReportManager"/>
    /// </summary>
    public abstract class ReportManager : IReportManager
    {
        #region Constants

        /// <summary>
        /// The report file base folder (without slash)
        /// </summary>
        protected const string BaseFolder = @"reports";

        /// <summary>
        /// The maximum number of retries
        /// </summary>
        protected const int MaximumNumberOfRetries = 3;

        #endregion Constants

        #region IReportManager fields

        public ReportExtension Extension { get; protected set; }
        public string FileName { get; protected set; }
        public string Path { get; protected set; }

        /// <summary>
        /// The report file base path (i.e. the folder, no <see cref="FileName"/>)
        /// </summary>
        [Obsolete("The report folder should be fixed (i.e. defined internally)")]
        public string BasePath
        {
            get => Directory.GetDirectoryRoot(Path);
            set => Path = $"{value}\\{FileName}{EnumToExtension()}";
        }

        #endregion IReportManager fields

        /// <summary>
        /// Initialize the <see cref="ReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        /// <param name="extension">The <see cref="ReportExtension"/></param>
        /// <remarks>To change the default <see cref="Path"/> use the property <see cref="BasePath"/></remarks>
        protected ReportManager(string fileName, ReportExtension extension)
        {
            FileName = fileName;
            Extension = extension;

            IoUtility.CreateDirectoryIfNotExists(BaseFolder);
            Path = $"{BaseFolder}\\{FileName}{EnumToExtension()}";
        }

        #region IReportManager methods (abstract)

        /// <summary>
        /// Add an <see cref="IReportEntry"/> to the report
        /// </summary>
        /// <param name="entry">The <see cref="IReportEntry"/></param>
        /// <returns>The <see cref="Task"/> that will add the <paramref name="entry"/></returns>
        public abstract Task<bool> AddEntry(IReportEntry entry);

        #endregion IReportManager methods (abstract)

        #region Protected methods

        /// <summary>
        /// Convert a <see cref="ReportExtension"/> to the relative extension with the dot (e.g. <see cref="ReportExtension.Txt"/> will be converted in ".txt")
        /// </summary>
        /// <returns>The converted <see cref="ReportExtension"/></returns>
        protected string EnumToExtension()
        {
            string extension = ".";

            switch (Extension)
            {
                case ReportExtension.Txt:
                    extension += "txt";
                    break;

                case ReportExtension.Csv:
                    extension += "csv";
                    break;

                case ReportExtension.Xlsx:
                    extension += "xlsx";
                    break;

                case ReportExtension.Pdf:
                    extension += "pdf";
                    break;

                case ReportExtension.Html:
                    extension += "html";
                    break;

                case ReportExtension.Xml:
                    extension += "xml";
                    break;

                case ReportExtension.Json:
                    extension += "json";
                    break;
            }

            return extension;
        }

        /// <summary>
        /// Check if a file is locked
        /// </summary>
        /// <returns><see langword="true"/> if the <paramref name="file"/> is not locked, <see langword="false"/> otherwise</returns>
        protected bool IsFileLocked() => IoUtility.IsFileLocked(Path);

        /// <summary>
        /// Save a report entry text asynchronously
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="saveMode">The <see cref="SaveMode"/></param>
        /// <returns>The (async) <see cref="Task"/></returns>
        protected async Task<bool> SaveEntryTextAsync(string text, SaveMode saveMode = SaveMode.Append)
        {
            int numberOfRetries = 0;

            if (IoUtility.DoesFileExist(Path))
                while (IsFileLocked() && numberOfRetries++ <= MaximumNumberOfRetries)
                    await Task.Delay(1000);

            bool fileUnlocked = numberOfRetries <= MaximumNumberOfRetries;
            if (fileUnlocked)
                await SaveAsync(text, Path, saveMode);
            else
                await Logger.WarnAsync($"Unable to add an entry to {Path} after {MaximumNumberOfRetries} tries");

            return fileUnlocked;
        }

        #endregion Protected methods
    }
}