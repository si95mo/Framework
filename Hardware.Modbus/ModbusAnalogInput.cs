using Core;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    public class ModbusAnalogInput : ModbusAnalogChannel
    {
        private int pollingInterval;
        private Task pollingTask;

        /// <summary>
        /// The polling interval in milliseconds
        /// </summary>
        public int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = value;
        }

        /// <summary>
        /// The polling <see cref="Task"/>
        /// </summary>
        internal Task PollingTask => pollingTask;

        public ModbusAnalogInput(string code, IResource resource, ushort address, int pollingInterval = 100, string measureUnit = "", string format = "0.000",
            NumericRepresentation representation = NumericRepresentation.Single, bool reverse = false) : base(code)
        {
            this.resource = resource;
            this.measureUnit = measureUnit;
            this.format = format;
            this.representation = representation;
            this.address = address;
            this.pollingInterval = pollingInterval;
            this.reverse = reverse;

            resource.Channels.Add(this);

            pollingTask = new Task(async () =>
                {
                    while (resource.Status == ResourceStatus.Executing)
                    {
                        await (resource as ModbusResource).Receive(code);
                        await Task.Delay(pollingInterval);
                    }
                }
            );
        }

        protected override async void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value); // Update connected properties value
            await (resource as ModbusResource).Receive(code);
        }
    }
}