using Core;
using System;

namespace Hardware.Modbus
{
    public class ModbusAnalogChannel : Channel<double>, IModbusChannel
    {
        protected ushort address;
        protected IResource resource;
        protected ModbusFunction function;
        protected NumericRepresentation representation;
        protected bool reverse;

        public ushort Address
        {
            get => address;
            set => address = value;
        }

        public IResource Resource
        {
            get => resource;
            set => resource = value;
        }

        public ModbusFunction Function
        {
            get => function;
            set => function = value;
        }

        public NumericRepresentation Representation
        {
            get => representation;
            set => representation = value;
        }

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