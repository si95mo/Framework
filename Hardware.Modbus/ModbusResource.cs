using Core;
using Core.DataStructures;
using Core.Threading;
using Diagnostic;
using Hardware.Resources;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Modbus
{
    /// <summary>
    /// Defines the modbus function
    /// </summary>
    public enum ModbusFunction
    {
        /// <summary>
        /// Read coil
        /// </summary>
        ReadCoil = 1,

        /// <summary>
        /// Read discrete input
        /// </summary>
        ReadDiscreteInput = 2,

        /// <summary>
        /// Read multiple holding registers
        /// </summary>
        ReadHoldingRegisters = 3,

        /// <summary>
        /// Read multiple input registers
        /// </summary>
        ReadInputRegisters = 4,

        /// <summary>
        /// Write single coil
        /// </summary>
        WriteSingleCoil = 5,

        /// <summary>
        /// Write single holding register
        /// </summary>
        WriteSingleHoldingRegister = 6,

        /// <summary>
        /// Write multiple coils
        /// </summary>
        WriteMultipleCoils = 15,

        /// <summary>
        /// Write multiple holding registers
        /// </summary>
        WriteMultipleHoldingRegister = 16
    }

    /// <summary>
    /// Implement a resource that communicates via modbus over TCP protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class ModbusResource : Resource
    {
        public override bool IsOpen => client.Connected;

        /// <summary>
        /// The <see cref="ModbusResource"/> ip address
        /// </summary>
        public string IpAddress { get; private set; }

        /// <summary>
        /// The polling <see cref="TimeSpan"> polling period
        /// </summary>
        public TimeSpan PollingPeriod { get; private set; }

        /// <summary>
        /// The <see cref="ModbusResource"/> port number
        /// </summary>
        public int Port { get; private set; }

        private TcpClient client;
        private ModbusIpMaster master;

        private readonly SemaphoreSlim semaphore;

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ModbusResource(string code) : base(code)
        {
            IpAddress = "127.0.0.1";
            Port = 502;

            client = new TcpClient()
            {
                ReceiveTimeout = 5000,
                SendTimeout = 5000
            };

            PollingPeriod = TimeSpan.FromMilliseconds(200d);
            semaphore = new SemaphoreSlim(1, 1);

            Status.Value = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        public ModbusResource() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        /// <param name="timeoutInMilliseconds">The send/receive timeout, in milliseconds</param>
        /// <param name="pollingPeriodInMilliseconds">The polling period, in milliseconds</param>
        public ModbusResource(string code, string ipAddress, int port = 502, int timeoutInMilliseconds = 5000, double pollingPeriodInMilliseconds = 250d) : this(code)
        {
            IpAddress = ipAddress;
            Port = port;

            client = new TcpClient
            {
                ReceiveTimeout = timeoutInMilliseconds,
                SendTimeout = timeoutInMilliseconds
            };

            PollingPeriod = TimeSpan.FromMilliseconds(pollingPeriodInMilliseconds);
        }

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        /// <param name="timeoutInMilliseconds">The send/receive timeout, in milliseconds</param>
        /// <param name="pollingTime">The polling period <see cref="TimeSpan"/></param>
        public ModbusResource(string code, string ipAddress, TimeSpan pollingTime, int port = 502, int timeoutInMilliseconds = 5000)
            : this(code, ipAddress, port, timeoutInMilliseconds, pollingTime.TotalMilliseconds)
        { }

        /// <summary>
        /// Send an update via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> of which send the value</param>
        internal async Task Send(string code)
        {
            try
            {
                await semaphore.WaitAsync();

                IChannel channel = Channels.Get(code);

                if (channel is ModbusAnalogOutput)
                {
                    NumericRepresentation representation = (channel as ModbusAnalogOutput).Representation;
                    double value = (channel as ModbusAnalogOutput).Value;
                    ushort[] values;

                    switch (representation)
                    {
                        case NumericRepresentation.Double:
                            values = ConvertFromDoubleToUshort(value);
                            if ((channel as ModbusAnalogChannel).Reverse)
                                Array.Reverse(values);

                            await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                            break;

                        case NumericRepresentation.Single:
                            values = ConvertFromSingleToUshort(value);
                            if ((channel as ModbusAnalogChannel).Reverse)
                                Array.Reverse(values);

                            await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                            break;

                        case NumericRepresentation.Int32:
                            values = ConvertFromIntToUshort(value);
                            if ((channel as ModbusAnalogChannel).Reverse)
                                Array.Reverse(values);

                            await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                            break;

                        case NumericRepresentation.UInt16:
                            await master.WriteSingleRegisterAsync((channel as ModbusAnalogOutput).Address, ConvertFromUshortToUshort(value));
                            break;
                    }
                }
                else
                {
                    if (channel is ModbusDigitalOutput)
                    {
                        await master.WriteSingleCoilAsync((channel as ModbusDigitalOutput).Address, (channel as ModbusDigitalOutput).Value);
                    }
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
            finally
            {
                semaphore.Release();
            }
        }

        /// <summary>
        /// Receive a value via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> in which store the value</param>
        internal async Task Receive()
        {
            Stopwatch timer = Stopwatch.StartNew();
            while (Status.Value == ResourceStatus.Executing)
            {
                try
                {
                    // Lock read until the write operation is done
                    await semaphore.WaitAsync();

                    #region Analog inputs

                    IEnumerable<ModbusAnalogInput> analogInputs = Channels.OfType<ModbusAnalogInput>();
                    ushort startAddress = analogInputs.Select((x) => x.Address).Min();
                    ushort bytesToRead = (ushort)analogInputs.Select(
                        (x) => x.Representation == NumericRepresentation.Byte ? 1 : (x.Representation == NumericRepresentation.Int16 ? 2 : 4)
                    ).Sum();

                    ushort[] analogBytes = await master.ReadHoldingRegistersAsync(startAddress, bytesToRead);
                    if (analogBytes.Length == analogInputs.Count())
                    {
                        int counter = 0;
                        foreach (ModbusAnalogInput input in analogInputs)
                        {
                            ushort[] buffer = new ushort[input.Representation == NumericRepresentation.Byte ? 1 : (input.Representation == NumericRepresentation.Int16 ? 2 : 4)];
                            Array.Copy(analogBytes, counter, buffer, 0, buffer.Length);

                            if (input.Reverse)
                            {
                                Array.Reverse(buffer);
                            }

                            input.Value = ConvertToDouble(buffer, input.Representation);

                            counter += buffer.Length;
                        }
                    }

                    #endregion Analog inputs

                    #region Digital inputs

                    IEnumerable<ModbusDigitalChannel> digitalInputs = Channels.OfType<ModbusDigitalInput>();
                    startAddress = digitalInputs.Select((x) => x.Address).Min();
                    bytesToRead = (ushort)digitalInputs.Count();

                    bool[] digitalValues = await master.ReadCoilsAsync(startAddress, bytesToRead);
                    if (digitalValues.Length == digitalInputs.Count())
                    {
                        int counter = 0;
                        foreach (ModbusDigitalChannel input in digitalInputs)
                        {
                            input.Value = digitalValues[counter++];
                        }
                    }

                    #endregion Digital inputs

                    timer.Stop();
                    TimeSpan timeToWait = PollingPeriod.Subtract(timer.Elapsed);
                    if (timeToWait.Ticks <= 0)
                    {
                        TimeSpan exceededTime = TimeSpan.FromTicks(-timeToWait.Ticks);
                        await Logger.WarnAsync($"Polling cycle exceeded the polling interval by {exceededTime}");
                    }
                    else
                    {
                        await Task.Delay(timeToWait);
                    }
                }
                catch(Exception ex)
                {
                    HandleException(ex);
                }
                finally
                {
                    semaphore.Release();
                    timer.Restart();
                }
            }
        }

        #region Start/Stop implementation

        /// <summary>
        /// Restart the <see cref="ModbusResource"/>
        /// </summary>
        public override async Task Restart()
        {
            Stop();
            await Start();
        }

        /// <summary>
        /// Start the <see cref="ModbusResource"/>
        /// </summary>
        public override async Task Start()
        {
            LastFailure.Clear();
            Status.Value = ResourceStatus.Starting;
            client = new TcpClient();

            try
            {
                await client.ConnectAsync(IpAddress, Port);

                if (client.Connected)
                {
                    master = ModbusIpMaster.CreateIp(client);

                    Status.Value = ResourceStatus.Executing;

                    Task receiveTask = Receive(); // Will execute until Status.Value = ResourceStatus.Executing
                    receiveTask.Start(); // Do not await, loop inside
                }
                else
                {
                    HandleException($"Unable to connect to {IpAddress}:{Port}");
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
            }
        }

        /// <summary>
        /// Stop the <see cref="ModbusResource"/>
        /// </summary>
        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            if (TestConnection() || IpAddress.CompareTo("127.0.0.1") == 0)
            {
                client.Close();
                Status.Value = ResourceStatus.Stopped;
            }

            if (Status.Value == ResourceStatus.Failure)
            {
                LastFailure = new Failure("Error occurred while closing the port!", DateTime.Now);
            }
        }

        #endregion Start/Stop implementation

        #region Conversion methods

        private ushort[] ConvertFromDoubleToUshort(double x)
        {
            ushort[] converted = new ushort[4];
            byte[] bytes = BitConverter.GetBytes(x);

            for (int i = 0; i < converted.Length; i++)
            {
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);
            }

            return converted;
        }

        private ushort[] ConvertFromSingleToUshort(double x)
        {
            ushort[] converted = new ushort[2];
            byte[] bytes = BitConverter.GetBytes((float)x);

            for (int i = 0; i < converted.Length; i++)
            {
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);
            }

            return converted;
        }

        private ushort[] ConvertFromIntToUshort(double x)
        {
            ushort[] converted = new ushort[2];
            byte[] bytes = BitConverter.GetBytes((int)x);

            for (int i = 0; i < converted.Length; i++)
            {
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);
            }

            return converted;
        }

        private ushort ConvertFromUshortToUshort(double x)
        {
            byte[] bytes = BitConverter.GetBytes((short)x);
            ushort converted = BitConverter.ToUInt16(bytes, 0);

            return converted;
        }

        private double ConvertToDouble(ushort[] values, NumericRepresentation representation)
        {
            double x = .0;
            int n = values.Length * sizeof(ushort), diff = 8 - n;
            byte[] bytes = new byte[n + diff];

            // Padding
            if (n != 8)
            {
                List<ushort> tmp = new List<ushort>();
                // int stop = 4 - values.Length;

                if (representation == NumericRepresentation.Int32 || representation == NumericRepresentation.UInt16)
                {
                    tmp.AddRange(values.ToList());
                }

                for (int i = 0; i < 4 - values.Length; i++)
                {
                    tmp.Add(0);
                }

                if (representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
                {
                    tmp.AddRange(values.ToList());
                }

                values = tmp.ToArray();
            }

            Buffer.BlockCopy(values, 0, bytes, 0, n + diff);

            if (representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
            {
                x = BitConverter.ToDouble(bytes, 0);
            }
            else
            {
                x += bytes[0];
                for (int i = 1; i < bytes.Length; i++)
                {
                    x += bytes[i] * (byte.MaxValue + 1) * i;
                }
            }

            return x;
        }

        #endregion Conversion methods

        /// <summary>
        /// Test for active tcp connection ad the
        /// specified <see cref="IpAddress"/> and
        /// <see cref="Port"/>
        /// </summary>
        /// <returns><see langword="true"/> if there is an active connection,
        /// <see langword="false"/> otherwise</returns>
        protected bool TestConnection()
        {
            bool result = false;

            try
            {
                if (IpAddress.CompareTo("localhost") != 0 && IpAddress.CompareTo("127.0.0.1") != 0)
                {
                    IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
                    TcpConnectionInformation[] tcpConnections = ipProperties.GetActiveTcpConnections().Where(
                        (x) => x.LocalEndPoint.Equals(client.Client?.LocalEndPoint) && x.RemoteEndPoint.Equals(client.Client?.RemoteEndPoint)
                    ).ToArray();

                    if (tcpConnections != null && tcpConnections.Length > 0)
                    {
                        TcpState stateOfConnection = tcpConnections.First().State;
                        if (stateOfConnection == TcpState.Established)
                        {
                            result = true;
                        }
                    }
                }
                else
                {
                    result = true;
                }

                return result;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                return result;
            }
        }
    }
}