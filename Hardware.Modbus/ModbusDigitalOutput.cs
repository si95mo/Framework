using Core;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implement a modbus digital output
    /// </summary>
    public class ModbusDigitalOutput : ModbusDigitalChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="ModbusDigitalOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The address</param>
        /// <param name="function">The modbus function</param>
        public ModbusDigitalOutput(string code, IResource resource, ushort address, ModbusFunction function = ModbusFunction.WriteSingleCoil)
            : base(code, resource, address, function, representation: NumericRepresentation.Boolean)
        {
            ChannelType = ChannelType.DigitalOutput;
            ValueChanged += ModbusDigitalOutput_ValueChanged;
        }

        private async void ModbusDigitalOutput_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            await (Resource as ModbusResource).Send(Code);
        }
    }
}