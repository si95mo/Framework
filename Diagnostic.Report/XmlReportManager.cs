using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define an xml <see cref="ReportManager"/>
    /// </summary>
    public class XmlReportManager : ReportManager
    {
        /// <summary>
        /// Create a new instance of <see cref="XmlReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public XmlReportManager(string fileName) : base(fileName, ReportExtension.Xml)
        { }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            XmlSerializer serializer = new XmlSerializer(entry.GetType());

            string text;
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, entry);
                text = writer.ToString();
            }

            bool succeded = await SaveEntryTextAsync(text);

            return succeded;
        }
    }
}