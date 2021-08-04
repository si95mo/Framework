using Core;

namespace Hardware.Resources
{
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