using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
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
            FlatAppearance.BorderColor = Colors.Black;
        }
    }
}