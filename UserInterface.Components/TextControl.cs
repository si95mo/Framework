using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a control for handling analog inputs as text.
    /// See <see cref="BaseControl"/>
    /// </summary>
    public partial class TextControl : BaseControl
    {
        public override object Value
        {
            get => txbValue.Text;
            set => txbValue.Text = (string)value;
        }

        /// <summary>
        /// Creates a new instance of <see cref="TextControl"/>
        /// </summary>
        public TextControl()
        {
            InitializeComponent();
            txbValue.Text = "";
        }

        /// <summary>
        /// The on load event handler
        /// </summary>
        /// <param name="e">The <see cref="EventArgs"/></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var topMargin = (txbValue.Size.Height / 2);
            txbValue.Margin = new Padding(0, topMargin, 0, 0);
        }
    }
}