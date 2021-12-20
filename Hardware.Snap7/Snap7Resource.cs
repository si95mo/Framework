using Core;
using Sharp7;
using System;
using System.Threading.Tasks;

namespace Hardware.Snap7
{
    /// <summary>
    /// Implement a <see cref="Resource"/> that communicate with
    /// Siemens PLC using the Snap7 protocol
    /// </summary>
    public class Snap7Resource : Resource
    {
        private string ipAddress;
        private int rack;
        private int slot;
        private int bufferSize;
        private byte[] buffer;

        private S7Client client;

        public override bool IsOpen => client.Connected;

        /// <summary>
        /// Create a new instance of <see cref="Snap7Resource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="rack">The rack number</param>
        /// <param name="slot">The slot number</param>
        /// <param name="bufferSize">The buffer size</param>
        public Snap7Resource(string code, string ipAddress, int rack, int slot, int bufferSize) : base(code)
        {
            this.ipAddress = ipAddress;
            this.rack = rack;
            this.slot = slot;

            this.bufferSize = bufferSize;
            buffer = new byte[bufferSize];

            client = new S7Client();
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override Task Start()
        {
            Status.Value = ResourceStatus.Starting;
            int result = client.ConnectTo(ipAddress, rack, slot);

            if (result == 0)
                Status.Value = ResourceStatus.Executing;
            else
            {
                string message = FormatErrorMessage(result);
                HandleException(message);
            }

            return Task.CompletedTask;
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;
            int result = client.Disconnect();

            if (result == 0)
                Status.Value = ResourceStatus.Stopped;
            else
            {
                string message = FormatErrorMessage(result);
                HandleException(message);
            }
        }

        /// <summary>
        /// Receive a value from the PLC and store it in the relative channel
        /// </summary>
        /// <param name="code">The channel code</param>
        /// <returns></returns>
        internal async Task Receive(string code)
        {
            ISnap7Channel channel = Channels.Get(code) as ISnap7Channel;

            if (Status.Value == ResourceStatus.Executing)
            {
                await Task.Run(() =>
                    {
                        int size = ExtractNumberOfBytes(channel);
                        int result = client.DBRead(channel.DataBlock, channel.MemoryAddress, size, buffer);

                        if (result == 0)
                        {
                            if (channel is Snap7DigitalInput)
                                (channel as Snap7DigitalInput).Value = Convert.ToBoolean(buffer[channel.MemoryAddress]);
                            else
                            {
                                // Memory buffer manipulation - get only the elements of interest
                                int n = ExtractNumberOfBytes(channel);
                                byte[] array = new byte[n];
                                Array.Copy(buffer, channel.MemoryAddress, array, 0, n);

                                // Channel value assignment
                                switch (channel.RepresentationBytes)
                                {
                                    case RepresentationBytes.Eight:
                                        (channel as Snap7AnalogInput).Value = BitConverter.ToDouble(array, 0);
                                        break;

                                    case RepresentationBytes.Four:
                                        switch ((channel as Snap7AnalogInput).NumericRepresentation)
                                        {
                                            case NumericRepresentation.Single:
                                                (channel as Snap7AnalogInput).Value = BitConverter.ToSingle(array, 0);
                                                break;

                                            case NumericRepresentation.Int32:
                                                (channel as Snap7AnalogInput).Value = BitConverter.ToUInt32(array, 0);
                                                break;
                                        }
                                        break;

                                    case RepresentationBytes.Two:
                                        (channel as Snap7AnalogInput).Value = BitConverter.ToUInt16(array, 0);
                                        break;

                                    case RepresentationBytes.One:
                                        (channel as Snap7AnalogInput).Value = array[0];
                                        break;
                                }
                            }
                        }
                        else
                        {
                            string message = FormatErrorMessage(result);
                            HandleException(message);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Send a value stored in the relative channel to the PLC
        /// </summary>
        /// <param name="code">The channel code</param>
        internal async Task Send(string code)
        {
            ISnap7Channel channel = channels.Get(code) as ISnap7Channel;

            if (Status.Value == ResourceStatus.Executing)
            {
                await Task.Run(() =>
                {
                    if (channel is Snap7DigitalOutput)
                        buffer[channel.MemoryAddress] = BitConverter.GetBytes((channel as Snap7DigitalOutput).Value)[0];
                    else
                    {
                        Snap7AnalogChannel analogChannel = channel as Snap7AnalogChannel;

                        int n = ExtractNumberOfBytes(channel);
                        byte[] array = new byte[n];

                        switch (analogChannel.NumericRepresentation)
                        {
                            case NumericRepresentation.Byte:
                                array = BitConverter.GetBytes((byte)analogChannel.Value);
                                break;

                            case NumericRepresentation.UInt16:
                                array = BitConverter.GetBytes((ushort)analogChannel.Value);
                                break;

                            case NumericRepresentation.Int32:
                                array = BitConverter.GetBytes((int)analogChannel.Value);
                                break;

                            case NumericRepresentation.Single:
                                array = BitConverter.GetBytes((float)analogChannel.Value);
                                break;

                            case NumericRepresentation.Double:
                                array = BitConverter.GetBytes(analogChannel.Value);
                                break;
                        }

                        Array.Copy(array, 0, buffer, channel.MemoryAddress, n);
                    }

                    int result = client.DBWrite(channel.DataBlock, channel.MemoryAddress, bufferSize, buffer);
                    if (result != 0)
                    {
                        string message = FormatErrorMessage(result);
                        HandleException(message);
                    }
                }
                );
            }
        }

        /// <summary>
        /// Format an error code into a readable message
        /// </summary>
        /// <param name="errorCode">The error code</param>
        /// <returns>The formatted message</returns>
        private string FormatErrorMessage(int errorCode)
        {
            string message = $"Unable to disconnect to the Siemens PLC at {ipAddress}! Error code: 0x{errorCode:X}{Environment.NewLine}" +
                $"Description: {client.ErrorText(errorCode)}";

            return message;
        }

        /// <summary>
        /// Extract the number of bytes based on <see cref="RepresentationBytes"/>
        /// </summary>
        /// <param name="channel">The <see cref="ISnap7Channel"/></param>
        /// <returns>The number of bytes</returns>
        private int ExtractNumberOfBytes(ISnap7Channel channel)
        {
            int numberOfBytes;

            switch (channel.RepresentationBytes)
            {
                case RepresentationBytes.One: // byte
                    numberOfBytes = 1;
                    break;

                case RepresentationBytes.Two: // ushort
                    numberOfBytes = 2;
                    break;

                case RepresentationBytes.Four: // int and float
                    numberOfBytes = 4;
                    break;

                case RepresentationBytes.Eight: // double
                    numberOfBytes = 8;
                    break;

                default:
                    numberOfBytes = 4;
                    break;
            }

            return numberOfBytes;
        }
    }
}