using System;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a report entry
    /// </summary>
    public interface IReportEntry
    {
        /// <summary>
        /// The entry value
        /// </summary>
        object Value { get; set; }

        /// <summary>
        /// The timestamp
        /// </summary>
        DateTime Timestamp { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The notes
        /// </summary>
        string Notes { get; set; }
    }
}