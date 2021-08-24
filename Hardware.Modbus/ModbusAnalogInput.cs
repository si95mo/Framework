using Core;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implement an analog input channel for modbus communication
    /// </summary>
    public class ModbusAnalogInput : ModbusAnalogChannel
    {
        private int pollingInterval;
        private readonly Task pollingTask;

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

        /// <summary>
        /// Create a new instance of <see cref="ModbusAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The address</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse bytes option</param>
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

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="ModbusAnalogInput"/>
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