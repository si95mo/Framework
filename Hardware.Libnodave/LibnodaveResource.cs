﻿using Core;
using System;
using System.Threading;
using System.Threading.Tasks;
using static libnodave;

namespace Hardware.Libnodave
{
    /// <summary>
    /// Implement a <see cref="Resource"/> that communicates with PLCs using LibNoDave
    /// </summary>
    public class LibnodaveResource : Resource
    {
        private string ipAddress;
        private int port;

        private daveOSserialType connectionType; // The connection type
        private daveInterface connectionInterface; // The connection interface
        private daveConnection connection; // The connection

        private byte[] buffer;
        private int bufferSize;
        private int rack;
        private int slot;

        private int timeout;

        private bool isOpen;

        public override bool IsOpen => isOpen;

        /// <summary>
        /// Create a new instance of <see cref="LibnodaveResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port</param>
        /// <param name="bufferSize">The buffer size (memory bytes to read)</param>
        /// <param name="rack">The rack number</param>
        /// <param name="slot">The slot number</param>
        /// <param name="timeout">The connection timeout (in milliseconds)</param>
        public LibnodaveResource(string code, string ipAddress, int port, int bufferSize, int rack, int slot, int timeout = 5000) : base(code)
        {
            this.ipAddress = ipAddress;
            this.port = port;

            this.bufferSize = bufferSize;
            buffer = new byte[bufferSize];

            this.rack = rack;
            this.slot = slot;

            this.timeout = timeout;

            isOpen = false;
        }

        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        public override async Task Start()
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Task connectionTask = new Task(() =>
                {
                    Status.Value = ResourceStatus.Starting;

                    connectionType.rfd = openSocket(port, ipAddress);
                    connectionType.wfd = connectionType.rfd;
                    connectionInterface = new daveInterface(connectionType, code, 0, daveProtoISOTCP, daveSpeed187k);

                    connectionInterface.initAdapter();
                    connection = new daveConnection(connectionInterface, 0, rack, slot);

                    int result = connection.connectPLC();
                    isOpen = result == 0;

                    if (result == 0)
                        Status.Value = ResourceStatus.Executing;
                    else
                    {
                        string message = FormatErrorMessage(result);
                        HandleException(message);
                    }
                },
                token
            );

            // Connection timeout handling
            Task waitingTask = new Task(() => Task.Delay(timeout));

            connectionTask.Start();
            //waitingTask.Start();
            //Task completedTask = await Task.WhenAny(connectionTask, waitingTask);

            //if (completedTask == waitingTask)
            //{
            //    tokenSource.Cancel();
            //    HandleException("Unable to connect to the PLC, timeout elapsed!");
            //}
        }

        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            int result = connection.disconnectPLC();
            isOpen = result == 0;

            if (result != 0)
            {
                string message = FormatErrorMessage(result);
                HandleException(message);
            }
            else
                Status.Value = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Send a value through the <see cref="LibnodaveResource"/>
        /// </summary>
        /// <param name="code">The channel code with the value to send</param>
        /// <returns>The async <see cref="Task"/></returns>
        internal async Task Send(string code)
        {
            ILibnodaveChannel channel = channels.Get(code) as ILibnodaveChannel;

            if (Status.Value == ResourceStatus.Executing)
            {
                await Task.Run(() =>
                    {
                        if (channel is LibnodaveDigitalOutput)
                            buffer[channel.MemoryAddress] = BitConverter.GetBytes((channel as LibnodaveDigitalOutput).Value)[0];
                        else
                        {
                            LibnodaveAnalogChannel analogChannel = channel as LibnodaveAnalogChannel;

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

                        int result = connection.writeBytes(daveInputs, 0, 0, bufferSize, buffer);
                        if (result != 0)
                        {
                            string message = FormatErrorMessage(result);
                            // HandleException(message);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Receive a value through the <see cref="LibnodaveResource"/>
        /// </summary>
        /// <param name="code">The channel code in which the value will be stored</param>
        /// <returns>The async <see cref="Task"/></returns>
        internal async Task Receive(string code)
        {
            ILibnodaveChannel channel = channels.Get(code) as ILibnodaveChannel;

            if (Status.Value == ResourceStatus.Executing)
            {
                await Task.Run(() =>
                    {
                        int result = connection.readBytes(daveInputs, 0, 0, bufferSize, buffer); // Read the memory buffer

                        if (result == 0) // Read ok
                        {
                            if (channel is LibnodaveDigitalInput)
                                (channel as LibnodaveDigitalInput).Value = Convert.ToBoolean(buffer[channel.MemoryAddress]);
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
                                        (channel as LibnodaveAnalogInput).Value = BitConverter.ToDouble(array, 0);
                                        break;

                                    case RepresentationBytes.Four:
                                        switch ((channel as LibnodaveAnalogChannel).NumericRepresentation)
                                        {
                                            case NumericRepresentation.Single:
                                                (channel as LibnodaveAnalogInput).Value = BitConverter.ToSingle(array, 0);
                                                break;

                                            case NumericRepresentation.Int32:
                                                (channel as LibnodaveAnalogInput).Value = BitConverter.ToUInt32(array, 0);
                                                break;
                                        }
                                        break;

                                    case RepresentationBytes.Two:
                                        switch ((channel as LibnodaveAnalogInput).NumericRepresentation)
                                        {
                                            case NumericRepresentation.UInt16:
                                                (channel as LibnodaveAnalogInput).Value = BitConverter.ToUInt16(array, 0);
                                                break;

                                            case NumericRepresentation.Int16:
                                                (channel as LibnodaveAnalogInput).Value = BitConverter.ToInt16(array, 0);
                                                break;
                                        }
                                        break;

                                    case RepresentationBytes.One:
                                        (channel as LibnodaveAnalogInput).Value = array[0];
                                        break;
                                }
                            }
                        }
                        else // Error during read
                        {
                            string message = FormatErrorMessage(result);
                            HandleException(message);
                        }
                    }
                );
            }
        }

        /// <summary>
        /// Extract the number of bytes based on <see cref="RepresentationBytes"/>
        /// </summary>
        /// <param name="channel">The <see cref="ILibnodaveChannel"/></param>
        /// <returns>The number of bytes</returns>
        private int ExtractNumberOfBytes(ILibnodaveChannel channel)
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

        /// <summary>
        /// Format an error code into a readable message
        /// </summary>
        /// <param name="errorCode">The error code</param>
        /// <returns>The formatted message</returns>
        private string FormatErrorMessage(int errorCode)
        {
            string message = $"Unable to perform the read operation! Error code: 0x{errorCode:X}{Environment.NewLine}" +
                $"\tDescription: {daveStrerror(errorCode)}";

            return message;
        }
    }
}