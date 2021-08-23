using Core.Scheduling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class LoadTestProgramPanel : UserControl
    {
        private InstructionScheduler scheduler;

        public LoadTestProgramPanel(InstructionScheduler scheduler, BaseControl parent)
        {
            InitializeComponent();

            Location = new Point(
                (Screen.PrimaryScreen.WorkingArea.Width - Width) / 2,
                (Screen.PrimaryScreen.WorkingArea.Height - Height) / 2
            );

            this.scheduler = scheduler;

            lsvTestPrograms.Columns.Add("Test program name", -2, HorizontalAlignment.Left);
            lsvTestPrograms.Columns.Add("Last modification date", -2, HorizontalAlignment.Left);
            lsvTestPrograms.GridLines = true;
            lsvTestPrograms.View = View.Details;

            GetFileNames();

            parent.Parent.Controls.Add(this);
            BringToFront();
        }

        private void GetFileNames()
        {
            List<string> fileNames = Directory.GetFiles(@"test\", "*.json")
                .Select(Path.GetFileNameWithoutExtension)
                .ToList();

            ListViewItem item;
            int counter = 0;
            foreach (string fileName in fileNames)
            {
                DateTime lastWriteTime = File.GetLastWriteTime(fileName);

                item = new ListViewItem(fileName);
                item.SubItems.Add(lastWriteTime.ToString("yyyy/MM/dd - HH:mm:ss"));

                lsvTestPrograms.Items.Add(item);

                counter = lsvTestPrograms.Items.Count;
                if (counter > 0)
                {
                    if (counter % 2 != 0)
                        lsvTestPrograms.Items[counter - 1].BackColor = Colors.LightBlue;
                    else
                        lsvTestPrograms.Items[counter - 1].BackColor = Colors.LightYellow;
                }
            }

        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Dispose();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnLoad.FlatAppearance.BorderColor = Colors.Black;

                ListViewItem itemSelected = lsvTestPrograms.SelectedItems[0];
                string selectedFile = itemSelected.Text;

                scheduler.LoadExecutionList(@"test\" + selectedFile + ".bin");

                Dispose();
            }
            catch(Exception)
            {
                btnLoad.FlatAppearance.BorderColor = Colors.Red;
                lsvTestPrograms.Focus();
            }
        }
    }
}
