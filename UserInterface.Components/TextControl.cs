using System;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define a control for handling analog inputs as text.
    /// See <see cref="BaseControl"/>
    /// </summary>
    public partial class TextControl : TextBox
    {
        /// <summary>
        /// Creates a new instance of <see cref="TextControl"/>
        /// </summary>
        public TextControl()
        {
            InitializeComponent();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            if (Parent != null) 
                BackColor = Parent.BackColor;

            base.OnParentChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            BackColor = Parent.BackColor;
            base.OnParentBackColorChanged(e);
        }
    }
}