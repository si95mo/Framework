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
        Xml = 5
    }

    /// <summary>
    /// Define a generic report manager (i.e. log a measure or a message that is not log-related to a file)
    /// </summary>
    public abstract class ReportManager
    {
        /// <summary>
        /// The <see cref="ReportManager"/> extension
        /// </summary>
        protected readonly ReportExtension Extension;

        /// <summary>
        /// Initialize the <see cref="ReportManager"/>
        /// </summary>
        /// <param name="extension">The <see cref="ReportExtension"/></param>
        protected ReportManager(ReportExtension extension)
        {
            Extension = extension;
        }

        /// <summary>
        /// Convert a <see cref="ReportExtension"/> to the relative extension with the dot (e.g. <see cref="ReportExtension.Txt"/> will be converted in ".txt")
        /// </summary>
        /// <returns>The converted <see cref="ReportExtension"/></returns>
        protected string EnumToExtension()
        {
            string extension = ".";

            switch(Extension)
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
            }

            return extension;
        }
    }
}
