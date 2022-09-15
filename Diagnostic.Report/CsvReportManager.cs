using System.Threading.Tasks;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a csv <see cref="ReportManager"/>
    /// </summary>
    public class CsvReportManager : ReportManager
    {
        public char Separator { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="CsvReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        /// <param name="separator">The csv separator</param>
        public CsvReportManager(string fileName, char separator = '\t') : base(fileName, ReportExtension.Csv)
        {
            Separator = separator;
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            string text = $"{entry.Timestamp}{Separator}{entry.Value}{Separator}{entry.Description}{Separator}{entry.Notes}";
            bool succeded = await SaveEntryTextAsync(text);

            return succeded;
        }
    }
}
