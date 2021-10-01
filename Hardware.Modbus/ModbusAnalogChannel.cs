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
        protected ushort address;

        /// <summary>
        /// The <see cref="IResource"/>
        /// </summary>
        protected IResource resource;

        /// <summary>
        /// The <see cref="ModbusFunction"/>
        /// </summary>
        protected ModbusFunction function;

        /// <summary>
        /// The <see cref="NumericRepresentation"/>
        /// </summary>
        protected NumericRepresentation representation;

        /// <summary>
        /// The reverse option
        /// </summary>
        protected bool reverse;

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
        protected ModbusAnalogChannel() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected ModbusAnalogChannel(string code) : base(code)
        { }
    }
}