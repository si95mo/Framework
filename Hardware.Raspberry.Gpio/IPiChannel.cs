using Core;
using Unosquare.RaspberryIO.Abstractions;

namespace Hardware.Raspberry
{
    /// <summary>
    /// Represent a basic prototype of a <see cref="PiResource"/> GPIO channel
    /// </summary>
    public interface IPiChannel : IProperty
    {
        /// <summary>
        /// The <see cref="IPiChannel"/> <see cref="GpioPinDriveMode"/>
        /// </summary>
        GpioPinDriveMode PinMode
        { get; }

        /// <summary>
        /// The <see cref="IPiChannel"/> pin number
        /// </summary>
        int PinNumber
        { get; set; }
    }
}