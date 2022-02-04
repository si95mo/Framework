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
        protected NumericRepresentation representation = NumericRepresentation.Boolean;

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
        protected ModbusDigitalChannel() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Initialize the class attributes with
        /// default parameters
        /// </summary>
        /// <param name="code">The code</param>
        protected ModbusDigitalChannel(string code) : base(code)
        { }
    }
}