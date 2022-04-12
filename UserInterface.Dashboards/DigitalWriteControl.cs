using Core;
using Core.DataStructures;
using Hardware;
using System;

namespace UserInterface.Dashboards
{
    public partial class DigitalWriteControl : DraggableControl, IDashboardControl<bool>
    {
        private DigitalOutput channel;
        private string channelCode;
        private string description;

        public IChannel Channel
        {
            get => channel;
            set
            {
                if (value is DigitalOutput)
                    channel = (DigitalOutput)value;
            }
        }

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

        public bool Value => channel.Value;

        public Type Type
        {
            get => GetType();
            set => _ = value;
        }

        public DigitalWriteControl()
        {
            InitializeComponent();

            channel = new DigitalOutput(Guid.NewGuid().ToString());
            description = "";
        }

        public void SetChannel(IChannel channel)
        {
            if (channel != null)
            {
                if (channel is DigitalOutput)
                {
                    this.channel.MeasureUnit = (channel as DigitalOutput).MeasureUnit;
                    this.channel.Format = (channel as DigitalOutput).Format;
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
            dgcChannelValue.Value = channel.Value;
        }

        private void Channel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                UpdateUserInterface();
            else
                BeginInvoke(new Action(() => Channel_ValueChanged(sender, e)));
        }

        private void DgcChannelValue_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            channel.Value = (bool)dgcChannelValue.Value;
        }
    }
}