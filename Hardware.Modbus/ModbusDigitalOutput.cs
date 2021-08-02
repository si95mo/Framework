using Core;

namespace Hardware.Modbus
{
    public class ModbusDigitalOutput : ModbusDigitalChannel
    {
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

        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value); // Update connected properties value
            (resource as ModbusResource).Send(code);
        }
    }
}