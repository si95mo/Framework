using Core;

namespace Hardware.Resources
{
    /// <summary>
    /// Class that represent a serial output
    /// to be used in serial communication. <br/>
    /// See also <see cref="SerialResource"/>
    /// </summary>
    public class SerialOutput : Channel<string>, ISerialChannel
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
        /// The <see cref="IResource"/>
        /// </summary>
        public IResource Resource
        {
            get => resource;
            set => resource = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="SerialOutput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="command">The command to send</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        public SerialOutput(string code, string command, IResource resource) : base(code)
        {
            this.command = command;
            this.resource = resource;

            value = "";
        }

        /// <summary>
        /// Propagate the new value assigned to the
        /// <see cref="SerialOutput"/>. <br/>
        /// This method is called when the value of the <see cref="SerialOutput"/>
        /// change and. in addition to propagate the new value, send it also
        /// via serial communication (using the underlying <see cref="Resource"/>)
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            (resource as SerialResource).Send(command + value);
            base.PropagateValues(sender, e);
        }
    }
}