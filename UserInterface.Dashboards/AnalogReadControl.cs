using Core;
using Core.DataStructures;
using Hardware;
using System;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// An <see cref="IDashboardControl"/> that executes analog reads
    /// </summary>
    public partial class AnalogReadControl : DraggableControl, IDashboardControl<double>
    {
        private AnalogInput channel;
        private string channelCode;
        private string description;

        public IChannel Channel
        {
            get => channel;
            set
            {
                if (value is AnalogInput || value is AnalogOutput)
                    channel = (AnalogInput)value;
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

        public double Value => channel.Value;

        /// <summary>
        /// Create a new instance of <see cref="AnalogReadControl"/>
        /// </summary>
        public AnalogReadControl()
        {
            InitializeComponent();

            channel = new AnalogInput(Guid.NewGuid().ToString());
            description = "";
        }

        public void SetChannel(IChannel channel)
        {
            if (channel != null)
            {
                if (channel is AnalogInput)
                {
                    this.channel.MeasureUnit = (channel as AnalogInput).MeasureUnit;
                    this.channel.Format = (channel as AnalogInput).Format;
                }
                else
                {
                    if (channel is AnalogOutput)
                    {
                        this.channel.MeasureUnit = (channel as AnalogOutput).MeasureUnit;
                        this.channel.Format = (channel as AnalogOutput).Format;
                    }
                }

                channel.ConnectTo(this.channel);
                channelCode = channel.Code;
                description = channel.Code;

                UpdateUserInterface();

                channel.ValueChanged += Channel_ValueChanged;
            }
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
            lblChannelValue.Text = channel.Value.ToString(channel.Format);
            lblChannelMeasureUnit.Text = $"[{channel.MeasureUnit}]";
        }

        private void Channel_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (!InvokeRequired)
                UpdateUserInterface();
            else
                BeginInvoke(new Action(() => Channel_ValueChanged(sender, e)));
        }
    }
}