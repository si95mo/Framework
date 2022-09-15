using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a json <see cref="ReportManager"/>
    /// </summary>
    public class JsonReportManager : ReportManager
    {
        /// <summary>
        /// Create a new instance of <see cref="JsonReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public JsonReportManager(string fileName) : base(fileName, ReportExtension.Json)
        { }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            string text = JsonConvert.SerializeObject(entry);
            bool succeded = await SaveEntryTextAsync(text);

            return succeded;
        }
    }
}
