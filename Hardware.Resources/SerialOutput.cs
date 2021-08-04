using Core;

namespace Hardware.Resources
{
    public class SerialOutput : Channel<string>, ISerialChannel
    {
        private string command;
        private IResource resource;

        public string Command
        {
            get => command;
            set => command = value;
        }

        public IResource Resource
        {
            get => resource;
            set => resource = value;
        }

        public SerialOutput(string code, string command, IResource resource) : base(code)
        {
            this.command = command;
            this.resource = resource;

            value = "";
        }

        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            (resource as SerialResource).Send(command + value);
            base.PropagateValues(sender, e);
        }
    }
}