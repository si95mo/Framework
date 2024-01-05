using System;

namespace Rest.TransferModel.System
{
    public class TimestampInformation
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
