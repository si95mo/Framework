using Core.Parameters;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define an <see cref="UserControl"/> for <see cref="IParameter"/>
    /// </summary>
    public partial class ParameterControl : UserControl
    {
        private IParameter parameter;
        private Panel parent;

        private ParameterControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of <see cref="ParameterControl"/>
        /// </summary>
        /// <remarks>
        /// It's necessary to pass the <paramref name="parent"/> container as a parameter
        /// in order to correctly resize the <see cref="ParameterControl"/>!
        /// </remarks>
        /// <param name="parameter">The <see cref="IParameter"/></param>
        /// <param name="parent">The <see cref="ChannelControl"/> parent <see cref="Panel"/></param>
        public ParameterControl(IParameter parameter, Panel parent = null) : this()
        {
            this.parameter = parameter;
            this.parent = parent;

            lblCode.Text = parameter.Code;
            lblValue.Text = AsciiToByte(parameter.ToString());
        }

        private void ParameterControl_Load(object sender, EventArgs e)
        {
            if (parent != null)
            {
                int width = parent.Size.Width - SystemInformation.VerticalScrollBarWidth - 4;
                int heigth = Size.Height;
                Size = new Size(width, heigth);
            }

            parameter.ValueChanged += Parameter_ValueChanged;
        }

        private void Parameter_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                lblValue.Text = AsciiToByte(parameter.ToString());
            else
                BeginInvoke(new Action(() => Parameter_ValueChanged(sender, e)));
        }

        /// <summary>
        /// Convert an ASCII <see cref="string"/> special characters with the relative <see cref="byte"/> representation
        /// </summary>
        /// <param name="ascii">The ASCII <see cref="string"/></param>
        /// <returns>The converted <see cref="string"/></returns>
        private string AsciiToByte(string ascii)
        {
            string toByte = ascii.Replace("\n", "0x10");
            return toByte;
        }
    }
}
