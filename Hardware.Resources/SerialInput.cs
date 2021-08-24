using Core;
using System.Text;

namespace Hardware.Resources
{
    /// <summary>
    /// Class that represent a serial input
    /// to be used in serial communication. <br/>
    /// See also <see cref="SerialResource"/>
    /// </summary>
    public class SerialInput : Channel<string>, ISerialChannel
    {
        private string command;
        private IResource resource;

        /// <summary>
        /// The command to send
        /// </summary>
        public string Command
        {
            get => command;
            set => command = value;
        }

        /// <summary>
        /// The <see cref="SerialInput"/>
        /// <see cref="IResource"/>
        /// </summary>
        public IResource Resource
        {
            get => resource;
            set => resource = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="SerialInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="command">The command to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public SerialInput(string code, string command, IResource resource) : base(code)
        {
            this.command = command;
            this.resource = resource;

            value = "";

            (resource as SerialResource).DataReceived += SerialInput_DataReceived;
        }

        private void SerialInput_DataReceived(object sender, DataReceivedArgs e)
        {
            // Value = (resource as SerialResource).Receive();
            Value = Encoding.Default.GetString(e.Data);
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="SerialInput"/>
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value);
        }
    }
}