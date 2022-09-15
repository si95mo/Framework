using System.Threading.Tasks;

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
    /// Define a basic structure for a report manager
    /// </summary>
    public interface IReportManager
    {
        /// <summary>
        /// The <see cref="IReportManager"/> extension
        /// </summary>
        ReportExtension Extension { get; }

        /// <summary>
        /// The <see cref="IReportManager"/> file name
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// The report file path
        /// </summary>
        string Path { get; }

        /// <summary>
        /// Add an <see cref="IReportEntry"/> to the report
        /// </summary>
        /// <param name="entry">The <see cref="IReportEntry"/></param>
        /// <returns>The <see cref="Task"/> that will add the <paramref name="entry"/></returns>
        Task<bool> AddEntry(IReportEntry entry);
    }
}