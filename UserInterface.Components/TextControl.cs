using System;
using System.ComponentModel;
using System.Drawing;
using System.Text.RegularExpressions;
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
        /// Set to <see langword="true"/> to allow only numeric-related <see cref="char"/>, <see langword="false"/> otherwise
        /// </summary>
        [Description("Allow only numerics characters"), Category("Data")]
        public bool NumericsOnly { get; set; }

        /// <summary>
        /// Creates a new instance of <see cref="TextControl"/>
        /// </summary>
        public TextControl()
        {
            InitializeComponent();

            NumericsOnly = false;
            Font = new Font("Lucida Sans Unicode", 12f);

            TextChanged += TextControl_TextChanged;
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

        private void TextControl_TextChanged(object sender, EventArgs e)
        {
            if (NumericsOnly) // If option enabled
            {
                bool match = Regex.IsMatch(Text, @"^[+-]?([0-9]+([.][0-9]*)?|[.][0-9]+)$"); // Get all digits characters plus +/- at the start and . in the middle
                if (!match && Text != string.Empty)
                {
                    Text = Text.Remove(Text.Length - 1);

                    // Set carrier at the end of the text
                    SelectionStart = Text.Length;
                    SelectionLength = 0;
                }
            }
        }
    }
}