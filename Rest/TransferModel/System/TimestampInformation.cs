using System;

namespace Rest.TransferModel.System
{
    public class TimestampInformation : Information
    {
        public DateTime Value { get; set; } = DateTime.MinValue;

        public TimestampInformation()
        { }

        public TimestampInformation(DateTime value)
        {
            Value = value;
        }
    }
}
