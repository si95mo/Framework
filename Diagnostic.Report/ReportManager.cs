using IO;
using System.Threading.Tasks;
using static IO.FileHandler;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define the type of record (i.e. the extension)
    /// </summary>
    public enum ReportExtension
    {
        /// <summary>
        /// The report will be saved as a txt
        /// </summary>
        Txt = 0,

        /// <summary>
        /// The report will be saved as a csv
        /// </summary>
        Csv = 1,

        /// <summary>
        /// The report will be saved as an xslx
        /// </summary>
        Xlsx = 2,

        /// <summary>
        /// The report will be saved as a pdf
        /// </summary>
        Pdf = 3,

        /// <summary>
        /// The report will be saved as a html
        /// </summary>
        Html = 4,

        /// <summary>
        /// The report will be saved as an xml
        /// </summary>
        Xml = 5,

        /// <summary>
        /// The report will be saved as a json
        /// </summary>
        Json = 6
    }

    /// <summary>
    /// Define a generic report manager (i.e. log a measure or a message that is not log-related to a file)
    /// </summary>
    public abstract class ReportManager
    {
        /// <summary>
        /// The report file base folder (without slash)
        /// </summary>
        protected const string BaseFolder = @"reports";

        /// <summary>
        /// The maximum number of retries
        /// </summary>
        protected const int MaximumNumberOfRetries = 3;

        /// <summary>
        /// The <see cref="ReportManager"/> extension
        /// </summary>
        protected readonly ReportExtension Extension;

        /// <summary>
        /// The <see cref="ReportManager"/> file name
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// The report file path
        /// </summary>
        public string Path { get; protected set; }

        /// <summary>
        /// Initialize the <see cref="ReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        /// <param name="extension">The <see cref="ReportExtension"/></param>
        protected ReportManager(string fileName, ReportExtension extension)
        {
            FileName = fileName;
            Extension = extension;

            IoUtility.CreateDirectoryIfNotExists("reports");
            Path = $"{BaseFolder}\\{FileName}.{EnumToExtension()}";
        }

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
        protected bool IsFileLocked() => IoUtility.IsFileLocked(FileName);

        /// <summary>
        /// Save a report entry text asynchronously
        /// </summary>
        /// <param name="text">The text to append</param>
        /// <param name="saveMode">The <see cref="SaveMode"/></param>
        /// <returns>The (async) <see cref="Task"/></returns>
        protected async Task<bool> SaveEntryTextAsync(string text, SaveMode saveMode = SaveMode.Append)
        {
            int numberOfRetries = 0;
            while (IsFileLocked() && numberOfRetries++ <= MaximumNumberOfRetries)
                await Task.Delay(1000);

            bool fileUnlocked = numberOfRetries <= MaximumNumberOfRetries;
            if (fileUnlocked)
                await SaveAsync(text, Path, saveMode);
            else
                await Logger.WarnAsync($"Unable to add an entry to {Path} after {MaximumNumberOfRetries} tries");

            return fileUnlocked;
        }

        /// <summary>
        /// Add an <see cref="IReportEntry"/> to the report
        /// </summary>
        /// <param name="entry">The <see cref="IReportEntry"/></param>
        /// <returns>The <see cref="Task"/> that will add the <paramref name="entry"/></returns>
        public abstract Task<bool> AddEntry(IReportEntry entry);
    }
}