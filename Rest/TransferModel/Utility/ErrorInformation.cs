using System;

namespace Rest.TransferModel.Utility
{
    public class ErrorInformation : Information
    {
        public string Message { get; set; } = string.Empty;
        public Exception Exception { get; set; } = default;

        public ErrorInformation()
        { }

        public ErrorInformation(string message)
        {
            Message = message;
        }

        public ErrorInformation(Exception exception) : this(exception.Message) 
        {
            Exception = exception;
        }

        public ErrorInformation(string message, Exception exception) : this(message)
        {
            Exception = exception;
        }
    }
}
