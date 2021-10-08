using Core;

namespace Hardware.Modbus
{
    /// <summary>
    /// Represent a basic prototype for a modbus channel
    /// </summary>
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
        ModbusFunction Function { get; }
    }
}