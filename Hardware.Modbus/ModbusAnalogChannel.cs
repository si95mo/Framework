using Core;
using System;

namespace Hardware.Modbus
{
    /// <summary>
    /// Define a generic analog channel that can be used
    /// in modbus communication. <br/>
    /// See also <see cref="ModbusResource"/>
    /// </summary>
    public class ModbusAnalogChannel : Channel<double>, IModbusChannel
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
        private NumericRepresentation representation;

        /// <summary>
        /// The reverse option
        /// </summary>
        private bool reverse;

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
        public ModbusFunction Function => function;

        /// <summary>
        /// The <see cref="NumericRepresentation"/>
        /// </summary>
        public NumericRepresentation Representation
        {
            get => representation;
            set => representation = value;
        }

        /// <summary>
        /// The reverse bytes option
        /// </summary>
        public bool Reverse
        {
            get => reverse;
            set => reverse = value;
        }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        protected ModbusAnalogChannel() : this(Guid.NewGuid().ToString(), null, 0, ModbusFunction.ReadHoldingRegisters)
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="address">The address</param>
        /// <param name="function">The <see cref="ModbusFunction"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <param name="representation">The <see cref="NumericRepresentation"/></param>
        /// <param name="reverse">The reverse option</param>
        protected ModbusAnalogChannel(string code, IResource resource, ushort address, ModbusFunction function, string measureUnit = "", string format = "0.0",
            NumericRepresentation representation = NumericRepresentation.Single, bool reverse = false)
            : base(code, measureUnit, format)
        {
            this.resource = resource;
            this.address = address;
            this.function = function;
            this.representation = representation;
            this.reverse = reverse;

            resource.Channels.Add(this);
        }
    }
}