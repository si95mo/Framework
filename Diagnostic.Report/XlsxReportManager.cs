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
    public class XlsxReportManager : ReportManager, IDisposable
    {
        private Application excel;
        private Workbook workbook;
        private Worksheet worksheet;
        private Worksheet sheet;

        private List<IReportEntry> entries;

        /// <summary>
        /// Create a new instance of <see cref="XlsxReportManager"/>
        /// </summary>
        /// <param name="fileName">The report file name (only the file name, no extension and full path)</param>
        public XlsxReportManager(string fileName) : base(fileName, ReportExtension.Xlsx)
        {
            excel = new Application
            {
                DisplayAlerts = false,
                Visible = false,
                UserControl = false
            };

            entries = new List<IReportEntry>();
        }

        public override async Task<bool> AddEntry(IReportEntry entry)
        {
            entries.Add(entry);
            await SaveEntries();

            return true;
        }

        public void Dispose()
        {
            workbook.Close(true);
        }

        private async Task SaveEntries()
        {
            workbook = IoUtility.DoesFileExist(Path) ? excel.Workbooks.Open(Path, ReadOnly: false) : excel.Workbooks.Add();
            worksheet = (Worksheet)workbook.Sheets[1];
            sheet = (Worksheet)workbook.ActiveSheet;

            int index = 1;

            // Headers
            sheet.Cells[index, 1].Value = "Timestamp";
            sheet.Cells[index, 2].Value = "Value";
            sheet.Cells[index, 3].Value = "Description";
            sheet.Cells[index, 4].Value = "Notes";

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
            excel.Quit();
        }
    }
}