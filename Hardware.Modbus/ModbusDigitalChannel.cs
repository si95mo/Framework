using Core;
using System;

namespace Hardware.Modbus
{
    /// <summary>
    /// Implement a modbus digital channel
    /// </summary>
    public class ModbusDigitalChannel : Channel<bool>, IModbusChannel
    {
        /// <summary>
        /// The address
        /// </summary>
        private ushort address;

        /// <summary>
        /// The <see cref="IResource"/>
        /// </summary>
        private IResource resource;

        /// <summary>
        /// The <see cref="ModbusFunction"/>
        /// </summary>
        private ModbusFunction function;

        /// <summary>
        /// The <see cref="NumericRepresentation"/>
        /// </summary>
        private NumericRepresentation representation = NumericRepresentation.Boolean;

        /// <summary>
        /// The address
        /// </summary>
        public ushort Address
        {
            get => address;
            set => address = value;
        }

        /// <summary>
        /// The <see cref="IResource"/>
        /// </summary>
        public IResource Resource
        {
            get => resource;
            set => resource = value;
        }

        /// <summary>
        /// The <see cref="ModbusFunction"/>
        /// </summary>
        public ModbusFunction Function
        {
            get => function;
            set => function = value;
        }

        /// <summary>
        /// The <see cref="NumericRepresentation"/>
        /// </summary>
        public NumericRepresentation Representation
        {
            get => representation;
            set => _ = value;
        }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        protected ModbusDigitalChannel() : this(Guid.NewGuid().ToString(), null, 0, ModbusFunction.ReadCoil)
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="address">The address</param>
        /// <param name="function">The <see cref="ModbusFunction"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse option</param>
        protected ModbusDigitalChannel(string code, IResource resource, ushort address, ModbusFunction function, string measureUnit = "", string format = "0.0",
            NumericRepresentation representation = NumericRepresentation.Single)
            : base(code, measureUnit, format)
        {
            this.resource = resource;
            this.address = address;
            this.function = function;
            this.representation = representation;

            Tags.Add("Modbus");
            resource.Channels.Add(this);
        }
    }
}