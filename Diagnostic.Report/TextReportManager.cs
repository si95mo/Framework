using System;
using System.Threading.Tasks;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a txt <see cref="ReportManager"/>
    /// </summary>
    public class TextReportManager : ReportManager
    {
        /// <summary>
        /// Create a new instance of <see cref="TextReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public TextReportManager(string fileName) : base(fileName, ReportExtension.Txt)
        { }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            string text = entry.ToString();
            bool succeded = await SaveEntryTextAsync($"{text}{Environment.NewLine}");

            return succeded;
        }
    }
}