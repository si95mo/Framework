using Core;
using Core.DataStructures;
using Hardware;
using System;
using System.Drawing;

namespace UserInterface.Dashboards
{
    public partial class DigitalReadControl : DraggableControl, IDashboardControl<bool>
    {
        private DigitalInput channel;
        private string description;

        public IChannel Channel => channel;

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

        public DigitalReadControl()
        {
            InitializeComponent();

            channel = new DigitalInput(Guid.NewGuid().ToString());
            description = "";
            led.Color = Color.Green;
        }

        /// <summary>
        /// Update the user interface components
        /// </summary>
        private void UpdateUserInterface()
        {
            lblChannelName.Text = description;
            led.On = Value;
        }

        public void SetChannel(IChannel channel)
        {
            if (channel != null)
            {
                channel.ConnectTo(this.channel);
                description = channel.Code;

                UpdateUserInterface();

                channel.ValueChanged += Channel_ValueChanged;
            }
        }

        public void SetChannel(string channelCode)
        {
            IChannel channelRetrieved = ServiceBroker.Get<IChannel>().Get(channelCode);
            SetChannel(channelRetrieved);
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
