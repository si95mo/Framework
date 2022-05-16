using Hardware;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    public partial class ChannelControl : UserControl
    {
        private IChannel channel;
        private Panel parent;

        /// <summary>
        /// Create a new instance of <see cref="ChannelControl"/>
        /// </summary>
        private ChannelControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Create a new instance of <see cref="ChannelControl"/>
        /// </summary>
        /// <remarks>
        /// It's necessary to pass the <paramref name="parent"/> container as a parameter
        /// in order to correctly resize the <see cref="ChannelControl"/>!
        /// </remarks>
        /// <param name="channel">The <see cref="IChannel"/></param>
        /// <param name="parent">The <see cref="ChannelControl"/> parent <see cref="Panel"/></param>
        public ChannelControl(IChannel channel, Panel parent = null) : this()
        {
            this.channel = channel;
            this.parent = parent;

            lblCode.Text = channel.Code;
            lblValue.Text = AsciiToByte(channel.ToString());
            lblTimestamp.Text = DateTime.Now.ToString("HH:mm:ss.fff");
        }

        private void ChannelControl_Load(object sender, EventArgs e)
        {
            if (parent != null)
            {
                int width = parent.Size.Width - SystemInformation.VerticalScrollBarWidth - 4;
                int heigth = Size.Height;
                Size = new Size(width, heigth);
            }

            channel.ValueChanged += Channel_ValueChanged;
        }

        private void Channel_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
            {
                lblValue.Text = AsciiToByte(channel.ToString());
                lblTimestamp.Text = DateTime.Now.ToString("HH:mm:ss.fff");
            }
            else
                BeginInvoke(new Action(() => Channel_ValueChanged(sender, e)));
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
