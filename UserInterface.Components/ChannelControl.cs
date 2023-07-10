using Hardware;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace UserInterface.Controls
{
    /// <summary>
    /// Define an <see cref="UserControl"/> for <see cref="IChannel"/>
    /// </summary>
    public partial class ChannelControl : UserControl
    {
        private IChannel channel;
        private Panel parent;

        private bool initialized = false;

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
            lblDescription.Text = channel.Description;
            lblValue.Text = AsciiToByte(channel.ToString());
            lblTimestamp.Text = DateTime.Now.ToString("HH:mm:ss.fff");

            string type = string.Empty;
            switch (channel.ChannelType)
            {
                case ChannelType.AnalogInput:
                    type = "AI";
                    break;

                case ChannelType.AnalogOutput:
                    type = "AO";
                    break;

                case ChannelType.DigitalInput:
                    type = "DI";
                    break;

                case ChannelType.DigitalOutput:
                    type = "DO";
                    break;

                case ChannelType.MultiSampleAnalogInput:
                    type = "MSAI";
                    break;

                case ChannelType.Stream:
                    type = "S";
                    break;
            }

            lblType.Text = type;

            lblTags.Text = string.Empty;
            channel.Tags.ForEach((x) => lblTags.Text += $"{x} ");
            lblTags.Text = lblTags.Text.Trim();

            initialized = true;
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

        protected override void OnResize(EventArgs e)
        {
            if (initialized)
            {
                base.OnResize(e);

                lblTimestamp.Location = new Point(Size.Width - lblTimestamp.Size.Width - 16, lblTimestamp.Location.Y);

                pnlTags.Location = new Point(lblTimestamp.Location.X - pnlTags.Size.Width - 2, lblTimestamp.Location.Y);
                //pnlTags.Size = new Size(pnlTags.Size.Width + offset - 8, pnlTags.Size.Height);

                lblValue.Location = new Point(pnlTags.Location.X - pnlTags.Size.Width - 2, lblTimestamp.Location.Y);
                //lblValue.Size = new Size(lblValue.Size.Width + offset - 8, lblValue.Size.Height);

                lblType.Location = new Point(lblValue.Location.X - lblValue.Size.Width - 2, lblTimestamp.Location.Y);
                //lblType.Size = new Size(lblType.Size.Width + offset - 8, lblType.Size.Height);

                lblCode.Location = new Point(2, lblTimestamp.Location.Y);
                //lblCode.Size = new Size(lblCode.Size.Width + offset - 8, lblCode.Size.Height);

                lblDescription.Location = new Point(lblCode.Location.X + lblCode.Size.Width + 2, lblTimestamp.Location.Y);
                int offset = lblType.Location.X - lblCode.Size.Width - lblCode.Location.X;
                lblDescription.Size = new Size(offset, lblDescription.Size.Height);
            }

            initialized = false;
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