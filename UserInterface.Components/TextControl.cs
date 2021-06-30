using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class TextControl : BaseControl
    {
        public override object Value
        {
            get => txbValue.Text;
            set => txbValue.Text = (string)value;
        }

        public TextControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var topMargin = (txbValue.Size.Height / 2);
            txbValue.Margin = new Padding(0, topMargin, 0, 0);
        }
    }
}
