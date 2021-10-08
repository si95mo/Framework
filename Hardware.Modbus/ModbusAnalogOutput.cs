using Core;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implement a modbus analog output
    /// </summary>
    public class ModbusAnalogOutput : ModbusAnalogChannel
    {
        /// <summary>
        /// Create a new instance of <see cref="ModbusAnalogOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The address</param>
        /// <param name="function">The <see cref="ModbusFunction"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse option</param>
        public ModbusAnalogOutput(string code, IResource resource, ushort address, ModbusFunction function = ModbusFunction.WriteSingleHoldingRegister, 
            string measureUnit = "", string format = "0.000", NumericRepresentation representation = NumericRepresentation.Single, bool reverse = false) 
            : base(code)
        {
            this.resource = resource;
            this.function = function;
            this.measureUnit = measureUnit;
            this.format = format;
            this.representation = representation;
            this.address = address;
            this.reverse = reverse;

            resource.Channels.Add(this);

            ValueChanged += ModbusAnalogOutput_ValueChanged;
        }

        private async void ModbusAnalogOutput_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            await (resource as ModbusResource).Send(code);
        }
    }
}