using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    public interface IModbusChannel : IProperty
    {
        /// <summary>
        /// The modbus register <see cref="NumericRepresentation"/>
        /// </summary>
        NumericRepresentation Representation { get; set; }

        /// <summary>
        /// The modbus register address
        /// </summary>
        ushort Address { get; set; }

        /// <summary>
        /// The modbus register <see cref="IResource"/>
        /// </summary>
        IResource Resource { get; set; }

        /// <summary>
        /// The modbus register <see cref="ModbusFunction"/>
        /// </summary>
        ModbusFunction Function { get; set; }
    }
}
