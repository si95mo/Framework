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
        public ModbusDigitalOutput(string code, IResource resource, ushort address, string measureUnit = "", string format = "0.000",
            NumericRepresentation representation = NumericRepresentation.Single) : base(code)
        {
            this.resource = resource;
            this.measureUnit = measureUnit;
            this.format = format;
            this.representation = representation;
            this.address = address;

            resource.Channels.Add(this);
        }

        /// <summary>
        /// Propagate the value changed event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            (resource as ModbusResource).Send(code);
            base.PropagateValues(sender, e);
        }
    }
}