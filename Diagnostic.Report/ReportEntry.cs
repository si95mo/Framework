using System;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define a report entry
    /// </summary>
    public class ReportEntry : IReportEntry
    {
        public DateTime Timestamp { get; set; }
        public object Value { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="ReportEntry"/>
        /// </summary>
        /// <remarks>The <see cref="Timestamp"/> is set to <see cref="DateTime.Now"/></remarks>
        /// <param name="value">The value</param>
        /// <param name="description">The description</param>
        /// <param name="notes">The notes</param>
        public ReportEntry(object value, string description, string notes)
        {
            Timestamp = DateTime.Now;
            Value = value;
            Description = description;
            Notes = notes;
        }

        /// <summary>
        /// Create a new instance of <see cref="ReportEntry"/>
        /// </summary>
        /// <param name="value">The value</param>
        /// <param name="timestamp">The timestamp</param>
        /// <param name="description">The description</param>
        /// <param name="notes">The notes</param>
        public ReportEntry(object value, DateTime timestamp, string description, string notes) : this(value, description, notes)
        {
            Timestamp = timestamp;
        }

        public override string ToString()
        {
            string description = $"{Timestamp:HH:mm:ss.fff} >> {Value}; {Description} ({Notes})";
            return description;
        }
    }
}