using Core;
using Core.DataStructures;
using Hardware;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace UserInterface.Dashboards
{
    public partial class AnalogWriteControl : DraggableControl, IDashboardControl<double>
    {
        private AnalogOutput channel;
        private string channelCode;
        private string description;

        public IChannel Channel => channel;

        public string ChannelCode
        {
            get => channelCode;
            set => channelCode = value;
        }

        public string Description
        {
            get => description;
            set
            {
                description = value;
                UpdateUserInterface();
            }
        }

        public double Value => channel.Value;

        public AnalogWriteControl()
        {
            InitializeComponent();

            channel = new AnalogOutput(Guid.NewGuid().ToString());
            description = "";
        }

        public void SetChannel(IChannel channel)
        {
            if (channel != null)
            {
                if (channel is AnalogOutput)
                {
                    this.channel.MeasureUnit = (channel as AnalogOutput).MeasureUnit;
                    this.channel.Format = (channel as AnalogOutput).Format;
                }
            }

            this.channel.ConnectTo(channel);
            channelCode = channel.Code;
            description = channel.Code;

            UpdateUserInterface();

            channel.ValueChanged += Channel_ValueChanged;
        }

        public void SetChannel(string channelCode)
        {
            IChannel retrievedChannel = ServiceBroker.Get<IChannel>().Get(channelCode);
            SetChannel(retrievedChannel);
        }

        /// <summary>
        /// Update the user interface components
        /// </summary>
        private void UpdateUserInterface()
        {
            lblChannelName.Text = description;
            lblChannelName.Left = (ClientSize.Width - lblChannelName.Width) / 2; // Center the label
            txbChannelValue.Text = channel.Value.ToString(channel.Format);
            lblChannelMeasureUnit.Text = $"[{channel.MeasureUnit}]";
        }

        private void TxbChannelValue_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                channel.Value = double.Parse(txbChannelValue.Text.TrimEnd(Environment.NewLine.ToCharArray()), CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Determines whether the <paramref name="text"/> is numeric
        /// </summary>
        /// <param name="text">The text to parse</param>
        /// <returns><see langword="true"/> if <paramref name="text"/> is numeric, <see langword="false"/> otherwise</returns>
        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); // Regex that matches disallowed text
            return regex.IsMatch(text);
        }

        private void TxbChannelValue_KeyPress(object sender, KeyPressEventArgs e)
            => e.Handled = IsTextAllowed(e.KeyChar.ToString());

        private void Channel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                UpdateUserInterface();
            else
                BeginInvoke(new Action(() => Channel_ValueChanged(sender, e)));
        }

        private void TxbChannelValue_MouseDown(object sender, MouseEventArgs e)
            => txbChannelValue.Text = "";
    }
}