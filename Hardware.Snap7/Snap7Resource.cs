using Core;
using Sharp7;
using System;
using System.Collections.Generic;
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
        private Dictionary<int, byte[]> dataBlocks;
        private int pollingInterval;

        private S7Client client;

        private object threadLock = new object();

        public override bool IsOpen => client.Connected;

        /// <summary>
        /// The polling interval (in milliseconds)
        /// </summary>
        public int PollingInterval
        {
            get => pollingInterval;
            set => pollingInterval = value;
        }

        /// <summary>
        /// Create a new instance of <see cref="Snap7Resource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="rack">The rack number</param>
        /// <param name="slot">The slot number</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public Snap7Resource(string code, string ipAddress, int rack, int slot, int pollingInterval = 100) : base(code)
        {
            this.ipAddress = ipAddress;
            this.rack = rack;
            this.slot = slot;
            this.pollingInterval = pollingInterval;

            dataBlocks = new Dictionary<int, byte[]>();
            client = new S7Client();
        }

        /// <summary>
        /// Add a new data block to read or reinitialize an existing one
        /// </summary>
        /// <param name="dataBlock">The data block number</param>
        /// <param name="size">The size of the associated buffer</param>
        public void AddDataBlock(int dataBlock, int size)
        {
            if (!dataBlocks.ContainsKey(dataBlock))
                dataBlocks.Add(dataBlock, new byte[size]);
            else
                dataBlocks[dataBlock] = new byte[size];
        }

        /// <summary>
        /// Get the associated data block buffer
        /// </summary>
        /// <param name="dataBlock">The data block of which get the buffer</param>
        /// <returns>
        /// The buffer, or <see langword="null"/> if <paramref name="dataBlock"/> was
        /// not previously specified
        /// </returns>
        internal byte[] GetDataBlockBuffer(int dataBlock)
        {
            byte[] buffer;

            if (dataBlocks.ContainsKey(dataBlock))
                buffer = dataBlocks[dataBlock];
            else
                buffer = null;

            return buffer;
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
            {
                Status.Value = ResourceStatus.Executing;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Receive();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
            else
            {
                string message = FormatErrorMessage(result, messageToPrepend: "On Start");
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
                string message = FormatErrorMessage(result, messageToPrepend: "On Stop");
                HandleException(message);
            }
        }

        /// <summary>
        /// Receive a value from the PLC and store it in the relative channel
        /// </summary>
        /// <returns>The async <see cref="Task"/></returns>
        private async Task Receive()
        {
            int result;
            string message;

            while (Status.Value == ResourceStatus.Executing)
            {
                foreach (int dataBlock in dataBlocks.Keys)
                {
                    // Read the data block for each one of the specified
                    lock (threadLock)
                        result = client.DBRead(dataBlock, 0, dataBlocks[dataBlock].Length, dataBlocks[dataBlock]);

                    if (result != 0)
                    {
                        message = FormatErrorMessage(result, messageToPrepend: "On Receive");
                        HandleException(message);
                    }
                }

                await Task.Delay(pollingInterval);
            }
        }

        /// <summary>
        /// Send a value stored in the relative channel to the PLC
        /// </summary>
        /// <param name="code">The channel code</param>
        internal void Send(string code)
        {
            ISnap7Channel channel = Channels.Get(code) as ISnap7Channel;

            if (Status.Value == ResourceStatus.Executing)
            {
                if (channel is Snap7DigitalOutput)
                    dataBlocks[channel.DataBlock][channel.MemoryAddress] = BitConverter.GetBytes((channel as Snap7DigitalOutput).Value)[0];
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

                        case NumericRepresentation.Int16:
                            array = BitConverter.GetBytes((short)analogChannel.Value);
                            break;
                    }

                    if ((channel as Snap7AnalogChannel).Reverse)
                        Array.Reverse(array);

                    Array.Copy(array, 0, dataBlocks[channel.DataBlock], channel.MemoryAddress, n);
                }

                int result;

                lock (threadLock)
                    result = client.DBWrite(channel.DataBlock, channel.MemoryAddress, dataBlocks[channel.DataBlock].Length, dataBlocks[channel.DataBlock]);

                if (result != 0)
                {
                    string message = FormatErrorMessage(result, messageToPrepend: "On Send");
                    HandleException(message);
                }
            }
        }

        /// <summary>
        /// Format an error code into a readable message
        /// </summary>
        /// <param name="errorCode">The error code</param>
        /// <param name="messageToPrepend">The message to prepend to the error one (if not equal to <see langword="null"/>)</param>
        /// <returns>The formatted message</returns>
        private string FormatErrorMessage(int errorCode, string messageToPrepend = null)
        {
            string message = $"Communication error with Siemens PLC at {ipAddress}! Error code: 0x{errorCode:X}{Environment.NewLine}" +
                $"Description: {client.ErrorText(errorCode)}";

            if (messageToPrepend != null)
                message = $"{messageToPrepend} >> {message}";

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