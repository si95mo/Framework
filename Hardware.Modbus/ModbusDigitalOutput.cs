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
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        public ModbusDigitalOutput(string code, IResource resource, ushort address) : base(code)
        {
            this.resource = resource;
            this.address = address;
            function = ModbusFunction.WriteSingleCoil;

            resource.Channels.Add(this);

            ValueChanged += ModbusDigitalOutput_ValueChanged;
        }

        private async void ModbusDigitalOutput_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            await(resource as ModbusResource).Send(code);
        }
    }
}