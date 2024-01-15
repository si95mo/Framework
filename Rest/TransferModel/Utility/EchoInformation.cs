using System;

namespace Rest.TransferModel.Utility
{
    public class EchoInformation : Information
    {
        public DateTime Timestamp { get; set; } = DateTime.MinValue;
        public string Value { get; set; } = string.Empty;

        public EchoInformation()
        { }

        public EchoInformation(string value)
        {
            Timestamp = DateTime.Now;
            Value = value;
        }
    }
}
