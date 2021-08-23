using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Represent a clock control for user interfaces
    /// </summary>
    public partial class ClockControl : BaseControl
    {
        private string timeFormat;
        private string dateFormat;

        private int controlWidth;
        private int controlHeigth;

        /// <summary>
        /// The actual <see cref="DateTime"/>
        /// </summary>
        public DateTime Now => DateTime.Now;

        /// <summary>
        /// Create a new instance of <see cref="ClockControl"/>
        /// </summary>
        public ClockControl()
        {
            InitializeComponent();

            timeFormat = "HH:mm:ss";
            dateFormat = "MMM dd, yyyy";

            controlWidth = Size.Width;
            controlHeigth = Size.Height;

            UpdateDateAndTime();

            Size timeLabelSize = lblTime.Size;
            Size dateLabelSize = lblDate.Size;

            lblTime.Location = new Point(
                (controlWidth - timeLabelSize.Width) / 2,
                (controlHeigth  - timeLabelSize.Height) / 2 - 20
            );

            lblDate.Location = new Point(
                (controlWidth - dateLabelSize.Width) / 2,
                (controlHeigth - dateLabelSize.Height) / 2 + 20
            );

            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateDateAndTime();
        }

        private void UpdateDateAndTime()
        {
            DateTime now = DateTime.Now;

            string time = now.ToString(timeFormat);
            string date = now.ToString(dateFormat, new CultureInfo("en-US"));

            lblTime.Text = time;
            lblDate.Text = date;
        }
    }
}
