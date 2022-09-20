using IO;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Diagnostic.Report
{
    /// <summary>
    /// Define an xlsx <see cref="ReportManager"/>
    /// </summary>
    public class XlsxReportManager : ReportManager
    {
        private List<IReportEntry> entries;

        /// <summary>
        /// Create a new instance of <see cref="XlsxReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public XlsxReportManager(string fileName) : base(fileName, ReportExtension.Xlsx)
        {
            entries = new List<IReportEntry>();
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            entries.Add(entry);
            await SaveEntries();

            return true;
        }

        private async Task SaveEntries()
        {
            Application excel = new Application();
            Workbook workbook = IoUtility.DoesFileExist(Path) ?
                excel.Workbooks.Open(Path, 0, false, 5, "", "", false, XlPlatform.xlWindows, "", true, false, 0, true, false, false) : 
                excel.Workbooks.Add();
            Worksheet worksheet = (Worksheet)workbook.Sheets[1];
            Worksheet sheet = (Worksheet)workbook.ActiveSheet;

            int index = 1;

            // Headers
            sheet.Cells[index, 1] = "Timestamp";
            sheet.Cells[index, 2] = "Value";
            sheet.Cells[index, 3] = "Description";
            sheet.Cells[index, 4] = "Notes";

            index++;

            // Data
            for (int i = 0; i < entries.Count; i++)
            {
                sheet.Cells[index, 1] = entries[i].Timestamp;
                sheet.Cells[index, 2] = entries[i].Value;
                sheet.Cells[index, 3] = entries[i].Description;
                sheet.Cells[index, 4] = entries[i].Notes;

                index++;
            }

            excel.Visible = false;
            excel.UserControl = false;

            await Task.Run(() =>
                workbook.SaveAs(
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

            workbook.Close(true);
        }
    }
}