using Core.DataStructures;
using Core.Parameters;
using Hardware;
using System;

namespace UserInterface.Dashboards
{
    public partial class MultiSampleAnalogReadControl : ResizableControl, IDashboardControl<double[]>
    {
        private AnalogInput analogInput;
        private MultiSampleAnalogInput multiSampleAnalogInput;
        private string channelCode;
        private string description;

        private NumericParameter numericParameter;
        private WaveformParameter waveformParameter;

        public double[] Value
        {
            get
            {
                double[] value = analogInput != null ? new double[] { analogInput.Value } : multiSampleAnalogInput.Value;
                return value;
            }
        }

        public IChannel Channel
        {
            get
            {
                IChannel channel;
                if (analogInput != null)
                    channel = analogInput;
                else
                    channel = multiSampleAnalogInput;

                return channel;
            }
            set
            {
                if (value is AnalogInput ai)
                    analogInput = ai;
                else
                {
                    if (value is MultiSampleAnalogInput msai)
                        multiSampleAnalogInput = msai;
                }
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

        public Type Type
        {
            get => GetType();
            set => _ = value;
        }

        public MultiSampleAnalogReadControl() : base()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Update the user interface components
        /// </summary>
        private void UpdateUserInterface()
        {
            lblChannelName.Text = description;
            lblChannelName.Left = (ClientSize.Width - lblChannelName.Width) / 2; // Center the label
        }

        public void SetChannel(IChannel channel)
        {
            if (channel != null)
            {
                if (channel is AnalogInput)
                {
                    analogInput = channel as AnalogInput;

                    numericParameter = new NumericParameter(channel.Code);
                    channel.ConnectTo(numericParameter);
                    chart.SetParameter(numericParameter);
                }
                else
                {
                    multiSampleAnalogInput = channel as MultiSampleAnalogInput;

                    waveformParameter = new WaveformParameter(channel.Code);
                    channel.ConnectTo(waveformParameter);
                    chart.SetParameter(waveformParameter);
                }

                channelCode = channel.Code;
                description = channel.Code;
            }
        }

        public void SetChannel(string channelCode)
        {
            IChannel retrievedChannel = ServiceBroker.Get<IChannel>().Get(channelCode);
            SetChannel(retrievedChannel);
        }
    }
}