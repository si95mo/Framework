using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a combo boc control
    /// </summary>
    public partial class ComboControl : BaseControl
    {
        /// <summary>
        /// The <see cref="ComboBox"/> of <see cref="ComboControl"/>
        /// </summary>
        public ComboBox Combo => cbxControl;

        /// <summary>
        /// The <see cref="ComboControl"/> value
        /// </summary>
        public override object Value => cbxControl.SelectedItem;

        /// <summary>
        /// The <see cref="ComboControl"/> selected item
        /// </summary>
        public object SelectedItem => cbxControl.SelectedItem;

        /// <summary>
        /// The <see cref="ComboControl"/> selected index
        /// </summary>
        public int SelectedIndex => cbxControl.SelectedIndex;

        /// <summary>
        /// The <see cref="ComboControl"/> <see cref="ComboBox.DataSource"/>
        /// </summary>
        public object DataSource
        {
            get => cbxControl.DataSource;
            set => cbxControl.DataSource = value;
        }

        /// <summary>
        /// The <see cref="ComboControl"/> <see cref="ControlBindingsCollection"/>
        /// </summary>
        public new ControlBindingsCollection DataBindings => cbxControl.DataBindings;

        /// <summary>
        /// Create a new instance of <see cref="ComboControl"/>
        /// </summary>
        public ComboControl()
        {
            InitializeComponent();

            //cbxControl.DropDownStyle = ComboBoxStyle.DropDownList;
            //cbxControl.DrawItem += ComboBox_DrawItem;
        }

        //private void ComboBox_DrawItem(object sender, DrawItemEventArgs e)
        //{
        //    if (e.Index < 0) return;
        //    Font f = cbxControl.Font;
        //    int yOffset = 10;

        //    if ((e.State & DrawItemState.Focus) == 0)
        //    {
        //        e.Graphics.FillRectangle(Brushes.White, e.Bounds);
        //        e.Graphics.DrawString(cbxControl.Items[e.Index].ToString(), f, Brushes.Black,
        //                              new Point(e.Bounds.X, e.Bounds.Y + yOffset));
        //    }
        //    else
        //    {
        //        e.Graphics.FillRectangle(Brushes.Blue, e.Bounds);
        //        e.Graphics.DrawString(cbxControl.Items[e.Index].ToString(), f, Brushes.White,
        //                              new Point(e.Bounds.X, e.Bounds.Y + yOffset));
        //    }
        //}
    }
}