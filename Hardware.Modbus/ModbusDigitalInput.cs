using Core;
using System;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implements a modbus digital input
    /// </summary>
    public class ModbusDigitalInput : ModbusDigitalChannel
    {
        private int pollingInterval;
        private Action pollingAction;

        /// <summary>
        /// The polling interval in milliseconds
        /// </summary>
        public int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = value;
        }

        /// <summary>
        /// The polling <see cref="Action"/>
        /// </summary>
        internal Action PollingAction => pollingAction;

        /// <summary>
        /// Create a new instance of <see cref="ModbusDigitalInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The modbus register address</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public ModbusDigitalInput(string code, IResource resource, ushort address, int pollingInterval = 100) : base(code)
        {
            this.resource = resource;
            this.address = address;
            function = ModbusFunction.ReadCoil;

            resource.Channels.Add(this);

            pollingAction = async () =>
            {
                while (true)
                {
                    await (resource as ModbusResource).Receive(code);
                    await Task.Delay(pollingInterval);
                }
            };
        }

        /// <summary>
        /// Propagate the value changed event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            await (resource as ModbusResource).Receive(code);
            base.PropagateValues(sender, e);
        }
    }
}