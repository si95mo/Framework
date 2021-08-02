using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    public class ModbusDigitalChannel : Channel<bool>, IModbusChannel
    {
        protected ushort address;
        protected IResource resource;
        protected ModbusFunction function;
        protected NumericRepresentation representation = NumericRepresentation.Boolean;

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
