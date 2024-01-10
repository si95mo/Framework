using Core;
using System;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implement an analog input channel for modbus communication
    /// </summary>
    public class ModbusAnalogInput : ModbusAnalogChannel
    {
        private int pollingInterval;
        private readonly Action pollingAction;

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
        /// Create a new instance of <see cref="ModbusAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The address</param>
        /// <param name="function">The <see cref="ModbusFunction"/></param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse bytes option</param>
        public ModbusAnalogInput(string code, IResource resource, ushort address, ModbusFunction function = ModbusFunction.ReadHoldingRegisters,
            int pollingInterval = 100, string measureUnit = "", string format = "0.000",
            NumericRepresentation representation = NumericRepresentation.Single, bool reverse = false)
            : base(code, resource, address, function, measureUnit, format, representation, reverse)
        {
            this.pollingInterval = pollingInterval;

            ChannelType = ChannelType.AnalogInput;
        }
    }
}