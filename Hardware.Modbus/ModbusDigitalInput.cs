using Core;
using System;

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
        /// <param name="function">The modbus function</param>
        public ModbusDigitalInput(string code, IResource resource, ushort address, int pollingInterval = 100, ModbusFunction function = ModbusFunction.ReadCoil)
            : base(code, resource, address, function, representation: NumericRepresentation.Boolean)
        {
            this.pollingInterval = pollingInterval;
            ChannelType = ChannelType.DigitalInput;
        }
    }
}