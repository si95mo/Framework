﻿using Core;
using System.Text;

namespace Hardware.Resources
{
    public class SerialInput : Channel<string>, ISerialChannel
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

        protected override void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            subscribers.ForEach(x => x.Value = Value);
        }
    }
}