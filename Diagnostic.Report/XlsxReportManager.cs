using Microsoft.Office.Interop.Excel;
using System;
using System.Threading.Tasks;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define an xlsx <see cref="ReportManager"/>
    /// </summary>
    public class XlsxReportManager : ReportManager
    {
        private int lastRow;

        /// <summary>
        /// Create a new instance of <see cref="XlsxReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public XlsxReportManager(string fileName) : base(fileName, ReportExtension.Xlsx)
        {
            lastRow = 1;
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            Application excel = new Application();
            Workbook workBook = excel.Workbooks.Open(Path);
            Worksheet workSheet = (Worksheet)workBook.Sheets[1];
            Worksheet sheet = (Worksheet)workBook.ActiveSheet;

            if (sheet.Cells[1, 1].CompareTo(string.Empty) == 0) // Empty cell, create headers
            {
                sheet.Cells[1, 1] = "Timestamp";
                sheet.Cells[1, 2] = "Value";
                sheet.Cells[1, 3] = "Description";
                sheet.Cells[1, 4] = "Notes";

                lastRow++;
            }

            // Appen
            sheet.Cells[lastRow, 1] = entry.Timestamp;
            sheet.Cells[lastRow, 2] = entry.Value;
            sheet.Cells[lastRow, 3] = entry.Description;
            sheet.Cells[lastRow, 4] = entry.Notes;

            lastRow++;

            excel.Visible = false;
            excel.UserControl = false;

            await Task.Run(() =>
                workBook.SaveAs(
                    Path,
                    XlFileFormat.xlWorkbookDefault,
                    Type.Missing,
                    Type.Missing,
                    false,
                    false,
                    XlSaveAsAccessMode.xlNoChange,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing,
                    Type.Missing
                )
            );

            return true;
        }
    }
}