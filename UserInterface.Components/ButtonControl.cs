using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Implement a button control
    /// </summary>
    public partial class ButtonControl : Button
    {
        /// <summary>
        /// Create a new instance of <see cref="ButtonControl"/>
        /// </summary>
        public ButtonControl()
        {
            InitializeComponent();

            BackColor = Colors.BackgroundColor;
            Size = new Size(150, 32);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderColor = Colors.Grey;
            FlatAppearance.MouseDownBackColor = Colors.Green;

            ForeColor = Colors.TextColor;
            BackColor = Colors.Grey;

            EnabledChanged += ButtonControl_EnabledChanged;
        }

        private void ButtonControl_EnabledChanged(object sender, System.EventArgs e)
            => BackColor = Enabled ? Colors.Grey : ControlPaint.Dark(Colors.Grey);
    }
}