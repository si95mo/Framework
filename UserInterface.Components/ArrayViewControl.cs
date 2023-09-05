using Newtonsoft.Json.Linq;
using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define an <see cref="UserControl"/> to visualize arrays
    /// </summary>
    public partial class ArrayViewControl : UserControl
    {
        /// <summary>
        /// The maximum number of items shown simultaneously by the control
        /// </summary>
        public const int MaxItemsShown = 10;

        /// <summary>
        /// The associated array
        /// </summary>
        public object[] ViewArray { get; set; }

        private TextControl[] TextControls = new TextControl[MaxItemsShown];
        private int offset;

        /// <summary>
        /// Create a new instance of <see cref="ArrayViewControl"/>
        /// </summary>
        public ArrayViewControl()
        {
            InitializeComponent();
            AutoScaleMode = AutoScaleMode.Inherit;

            int i = 0;
            TextControls[i++] = txb1;
            TextControls[i++] = txb2;
            TextControls[i++] = txb3;
            TextControls[i++] = txb4;
            TextControls[i++] = txb5;
            TextControls[i++] = txb6;
            TextControls[i++] = txb7;
            TextControls[i++] = txb8;
            TextControls[i++] = txb9;
            TextControls[i++] = txb10;

            offset = 0;
        }

        public ArrayViewControl(object[] array) : this()
        {
            SetArray(array);
        }

        /// <summary>
        /// Set a new array and visualize it
        /// </summary>
        /// <param name="array">The new array to set</param>
        public void SetArray(object[] array)
        {
            ViewArray = array;
            UpdateView(ViewArray);
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            if (offset > 0)
            {
                offset--;
                int n = Math.Min(MaxItemsShown, ViewArray.Length - offset);
                object[] buffer = new object[MaxItemsShown];

                Array.Copy(ViewArray, offset, buffer, 0, n);
                UpdateView(buffer);
            }
            else
                UpdateView(new[] { ViewArray[0] }); // Locked on first element
        }

        private void BtnClick_Next(object sender, EventArgs e)
        {
            if (offset < ViewArray.Length - 1)
            {
                offset++;
                int n = Math.Min(MaxItemsShown, ViewArray.Length - offset);
                object[] buffer = new object[MaxItemsShown];

                Array.Copy(ViewArray, offset, buffer, 0, n);
                UpdateView(buffer);
            }
            else
                UpdateView(new[] { ViewArray[ViewArray.Length - 1] });
        }

        private void UpdateView(object[] array)
        {
            if (!InvokeRequired)
            {
                int n = Math.Min(MaxItemsShown, array.Length);
                for (int i = 0; i < n; i++)
                    TextControls[i].Text = array[i] != null ? array[i].ToString() : string.Empty;
            }
            else
                BeginInvoke(new Action(() => UpdateView(array))); // Locked on last element
        }


    }
}
