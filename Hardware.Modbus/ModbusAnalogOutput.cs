using Core;

namespace Hardware.Modbus
{
    public class ModbusAnalogOutput : ModbusAnalogChannel
    {
        public ModbusAnalogOutput(string code, IResource resource, ushort address, string measureUnit = "", string format = "0.000",
            NumericRepresentation representation = NumericRepresentation.Single, bool reverse = false) : base(code)
        {
            this.resource = resource;
            this.measureUnit = measureUnit;
            this.format = format;
            this.representation = representation;
            this.address = address;
            this.reverse = reverse;

            resource.Channels.Add(this);
        }

        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value); // Update connected properties value
            (resource as ModbusResource).Send(code);
        }
    }
}