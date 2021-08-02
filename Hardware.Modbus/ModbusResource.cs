using Core;
using Core.DataStructures;
using Hardware.Resources;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
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
    /// Implement a resource that communicates via modbus over tcp protocol.
    /// See also <see cref="IResource"/>
    /// </summary>
    public class ModbusResource : TcpResource
    {
        private ModbusIpMaster master;

        /// <summary>
        /// The <see cref="ModbusResource"/> value as <see cref="object"/>
        /// </summary>
        public new object ValueAsObject
        {
            get => code;
            set
            {
                _ = ValueAsObject;
            }
        }

        /// <summary>
        /// The <see cref="ModbusResource"/> port number
        /// </summary>
        public new int Port => port;

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ModbusResource(string code)
        {
            this.code = code;
            ipAddress = "";
            port = 0;
            failure = new Failure();
            channels = new Bag<IChannel>();

            tcp = new TcpClient();

            status = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        public ModbusResource() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="ipAddress">The ip address</param>
        /// <param name="port">The port number</param>
        public ModbusResource(string code, string ipAddress, int port = 502)
        {
            this.code = code;
            this.ipAddress = ipAddress;
            this.port = port;
            failure = new Failure();
            channels = new Bag<IChannel>(); 

            tcp = new TcpClient();

            status = ResourceStatus.Stopped;
        }

        /// <summary>
        /// Send an update via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> of which send the value</param>
        internal new async void Send(string code)
        {
            var channel = channels.Get(code);

            if (channel is ModbusAnalogOutput)
            {
                NumericRepresentation representation = (channel as ModbusAnalogOutput).Representation;
                double value = (channel as ModbusAnalogOutput).Value;
                ushort[] values;

                switch (representation)
                {
                    case NumericRepresentation.Double:
                        values = ConvertFromDoubleToUInt16(value);
                        if ((channel as ModbusAnalogChannel).Reverse)
                            Array.Reverse(values);

                        await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                        break;

                    case NumericRepresentation.Single:
                        values = ConvertFromSingleToUInt16(value);
                        if ((channel as ModbusAnalogChannel).Reverse)
                            Array.Reverse(values);

                        await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                        break;

                    case NumericRepresentation.Int32:
                        values = ConvertFromInt32ToUInt16(value);
                        if ((channel as ModbusAnalogChannel).Reverse)
                            Array.Reverse(values);

                        await master.WriteMultipleRegistersAsync((channel as ModbusAnalogOutput).Address, values);
                        break;

                    case NumericRepresentation.UInt16:
                        await master.WriteSingleRegisterAsync((channel as ModbusAnalogOutput).Address, ConvertFromUInt16ToUInt16(value));
                        break;
                }
            }
            else
                if(channel is ModbusDigitalOutput)
                    await master.WriteSingleCoilAsync((channel as ModbusDigitalOutput).Address, (channel as ModbusDigitalOutput).Value);
        }

        /// <summary>
        /// Receive a value via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> in which store the value</param>
        internal async Task Receive(string code)
        {
            var channel = channels.Get(code);

            if (channel is ModbusAnalogInput)
            {
                NumericRepresentation representation = (channel as ModbusAnalogInput).Representation;
                ushort[] values;

                switch (representation)
                {
                    case NumericRepresentation.Double:
                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 4);
                        if ((channel as ModbusAnalogInput).Reverse)
                            Array.Reverse(values);

                        (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                        break;

                    case NumericRepresentation.Int32:
                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 2);
                        if ((channel as ModbusAnalogInput).Reverse)
                            Array.Reverse(values);

                        (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                        break;

                    case NumericRepresentation.Single:
                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 2);
                        if ((channel as ModbusAnalogInput).Reverse)
                            Array.Reverse(values);

                        (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                        break;

                    case NumericRepresentation.UInt16:
                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 1);
                        if ((channel as ModbusAnalogInput).Reverse)
                            Array.Reverse(values);

                        (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                        break;
                }
            }
            else
            {
                if (channel is ModbusDigitalInput)
                {
                    bool value = (await master?.ReadCoilsAsync((channel as ModbusDigitalInput).Address, 1))[0];
                    (channel as ModbusDigitalInput).Value = value;                        
                }
            }
        }

        /// <summary>
        /// Restart the <see cref="ModbusResource"/>
        /// </summary>
        public new void Restart()
        {
            Stop();
            Start();
        }

        /// <summary>
        /// Start the <see cref="ModbusResource"/>
        /// </summary>
        public new async void Start()
        {
            failure.Clear();
            status = ResourceStatus.Starting;

            if (TestConnection() || ipAddress.CompareTo("127.0.0.1") == 0)
            {
                await tcp.ConnectAsync(ipAddress, port);
                master = ModbusIpMaster.CreateIp(tcp);

                status = ResourceStatus.Executing;

                foreach(var channelCode in channels)
                {
                    var channel = channels.Get(channelCode);

                    if (channel is ModbusAnalogInput)
                        (channel as ModbusAnalogInput).PollingTask.Start();

                    if(channel is ModbusDigitalInput)
                        (channel as ModbusDigitalInput).PollingTask.Start();
                }
            }
            else
                status = ResourceStatus.Failure;

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while opening the port!", DateTime.Now);
        }

        /// <summary>
        /// Stop the <see cref="ModbusResource"/>
        /// </summary>
        public new void Stop()
        {
            status = ResourceStatus.Stopping;

            if (TestConnection() || ipAddress.CompareTo("127.0.0.1") == 0)
            {
                tcp.Close();
                status = ResourceStatus.Stopped;

                foreach (var channelCode in channels)
                {
                    var channel = channels.Get(channelCode);

                    if (channel is ModbusAnalogInput)
                        (channel as ModbusAnalogInput).PollingTask.Dispose();

                    if (channel is ModbusDigitalInput)
                        (channel as ModbusDigitalInput).PollingTask.Dispose();
                }
            }

            if (status == ResourceStatus.Failure)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);
        }

        private ushort[] ConvertFromDoubleToUInt16(double x)
        {
            ushort[] converted = new ushort[4];
            byte[] bytes = BitConverter.GetBytes(x);

            for(int i = 0; i < converted.Length; i++)
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);

            return converted;
        }

        private ushort[] ConvertFromSingleToUInt16(double x)
        {
            ushort[] converted = new ushort[2];
            byte[] bytes = BitConverter.GetBytes((float)x);

            for (int i = 0; i < converted.Length; i++)
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);

            return converted;
        }

        private ushort[] ConvertFromInt32ToUInt16(double x)
        {
            ushort[] converted = new ushort[2];
            byte[] bytes = BitConverter.GetBytes((int)x);

            for (int i = 0; i < converted.Length; i++)
                converted[i] = BitConverter.ToUInt16(bytes, i * 2);

            return converted;
        }

        private ushort ConvertFromUInt16ToUInt16(double x)
        {
            ushort converted = 0;
            byte[] bytes = BitConverter.GetBytes((short)x);

            converted = BitConverter.ToUInt16(bytes, 0);

            return converted;
        }

        private double ConvertFromUInt16ToDouble(ushort[] values, NumericRepresentation representation)
        {
            double x = .0;
            int n = values.Length * sizeof(ushort), diff = 8 - n;
            byte[] bytes = new byte[n + diff];

            // Padding
            if(n != 8)
            {
                List<ushort> tmp = new List<ushort>();
                int start = 0, stop = 4 - values.Length;

                if(representation == NumericRepresentation.Int32 || representation == NumericRepresentation.UInt16)
                    tmp.AddRange(values.ToList());

                for (int i = 0; i < 4 - values.Length; i++)
                    tmp.Add(0);

                if (representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
                    tmp.AddRange(values.ToList());

                values = tmp.ToArray();
            }

            Buffer.BlockCopy(values, 0, bytes, 0, n + diff);

            if(representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
                x = BitConverter.ToDouble(bytes, 0);
            else
            {
                x += bytes[0];
                for (int i = 1; i < bytes.Length; i++)
                    x += bytes[i] * (byte.MaxValue + 1) * i;
            }

            return x;
        }
    }
}