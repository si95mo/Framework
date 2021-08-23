using Core;

namespace Hardware.Resources
{
    /// <summary>
    /// Represent a basic serial channel prototype. <br/>
    /// See also <see cref="SerialResource"/>
    /// </summary>
    public interface ISerialChannel : IProperty<string>
    {
        /// <summary>
        /// The command to send using <see cref="SerialResource"/>
        /// </summary>
        string Command
        { get; set; }

        /// <summary>
        /// The serial <see cref="IResource"/>
        /// </summary>
        IResource Resource { get; set; }
    }
}