using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class ButtonControl : Button
    {
        public ButtonControl()
        {
            InitializeComponent();

            BackColor = Colors.BackgroundColor;
            Size = new Size(150, 25);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderColor = Colors.Black;
        }
    }
}