using Core;
using Core.Threading;
using Diagnostic;
using Hardware.Resources;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
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
        private bool writing;
        private bool started;

        /// <summary>
        /// Create a new instance of <see cref="ModbusResource"/>
        /// </summary>
        /// <param name="code">The code</param>
        public ModbusResource(string code) : base(code)
        {
            Status.Value = ResourceStatus.Stopped;
            started = false;
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
        /// <param name="timeout"></param>
        public ModbusResource(string code, string ipAddress, int port = 502, int timeout = 5000) : base(code, ipAddress, port)
        {
            tcp.ReceiveTimeout = timeout;
            tcp.SendTimeout = timeout;

            Status.Value = ResourceStatus.Stopped;

            writing = false;
        }

        /// <summary>
        /// Send an update via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> of which send the value</param>
        internal new async Task Send(string code)
        {
            if (started)
            {
                try
                {
                    writing = true;
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
                        if (channel is ModbusDigitalOutput)
                        master.WriteSingleCoil((channel as ModbusDigitalOutput).Address, (channel as ModbusDigitalOutput).Value);

                    writing = false;
                }
                catch (Exception ex)
                {
                    failure = new Failure(ex.Message);
                    Status.Value = ResourceStatus.Failure;
                    Logger.Log(ex);

                    writing = false;
                    started = false;
                }
            }
        }

        /// <summary>
        /// Receive a value via the modbus protocol
        /// </summary>
        /// <param name="code">The code of the <see cref="IModbusChannel"/> in which store the value</param>
        internal async Task Receive(string code)
        {
            if (started)
            {
                try
                {
                    int counter = 0;
                    while (writing && ++counter < 100)
                        await Tasks.NoOperation(1);

                    var channel = channels.Get(code);

                    if (channel is ModbusAnalogInput)
                    {
                        NumericRepresentation representation = (channel as ModbusAnalogInput).Representation;
                        ushort[] values = null;

                        switch (representation)
                        {
                            case NumericRepresentation.Double:
                                switch ((channel as IModbusChannel).Function)
                                {
                                    case ModbusFunction.ReadHoldingRegisters:
                                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 4);
                                        break;

                                    case ModbusFunction.ReadInputRegisters:
                                        values = await master?.ReadInputRegistersAsync((channel as ModbusAnalogChannel).Address, 4);
                                        break;
                                }

                                if ((channel as ModbusAnalogInput).Reverse)
                                    Array.Reverse(values);

                                (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                                break;

                            case NumericRepresentation.Int32:
                                switch ((channel as IModbusChannel).Function)
                                {
                                    case ModbusFunction.ReadHoldingRegisters:
                                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 2);
                                        break;

                                    case ModbusFunction.ReadInputRegisters:
                                        values = await master?.ReadInputRegistersAsync((channel as ModbusAnalogChannel).Address, 2);
                                        break;
                                }

                                if ((channel as ModbusAnalogInput).Reverse)
                                    Array.Reverse(values);

                                (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                                break;

                            case NumericRepresentation.Single:
                                switch ((channel as IModbusChannel).Function)
                                {
                                    case ModbusFunction.ReadHoldingRegisters:
                                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 2);
                                        break;

                                    case ModbusFunction.ReadInputRegisters:
                                        values = await master?.ReadInputRegistersAsync((channel as ModbusAnalogChannel).Address, 2);
                                        break;
                                }

                                if ((channel as ModbusAnalogInput).Reverse)
                                    Array.Reverse(values);

                                (channel as ModbusAnalogInput).Value = ConvertFromUInt16ToDouble(values, (channel as ModbusAnalogInput).Representation);
                                break;

                            case NumericRepresentation.UInt16:
                                switch ((channel as IModbusChannel).Function)
                                {
                                    case ModbusFunction.ReadHoldingRegisters:
                                        values = await master?.ReadHoldingRegistersAsync((channel as ModbusAnalogInput).Address, 1);
                                        break;

                                    case ModbusFunction.ReadInputRegisters:
                                        values = await master?.ReadInputRegistersAsync((channel as ModbusAnalogChannel).Address, 1);
                                        break;
                                }

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
                            bool value = false;
                            switch ((channel as IModbusChannel).Function)
                            {
                                case ModbusFunction.ReadCoil:
                                    value = (await master?.ReadCoilsAsync((channel as ModbusDigitalInput).Address, 1))[0];
                                    break;
                            }

                            (channel as ModbusDigitalInput).Value = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    failure = new Failure(ex.Message);
                    Status.Value = ResourceStatus.Failure;
                    Logger.Log(ex);

                    started = false;
                }
            }
        }

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
            failure.Clear();
            Status.Value = ResourceStatus.Starting;
            tcp = new TcpClient();

            try
            {
                IAsyncResult result = tcp.BeginConnect(ipAddress, port, null, null);
                bool success = result.AsyncWaitHandle.WaitOne(5000);

                if (success)
                {
                    master = ModbusIpMaster.CreateIp(tcp);

                    Status.Value = ResourceStatus.Executing;
                    started = true;

                    foreach (IProperty channel in channels)
                    {
                        if (channel is ModbusAnalogInput)
                            await Task.Factory.StartNew((channel as ModbusAnalogInput).PollingAction);

                        if (channel is ModbusDigitalInput)
                            await Task.Factory.StartNew((channel as ModbusDigitalInput).PollingAction);
                    }
                }
                else
                {
                    failure = new Failure($"Unable to connect to {ipAddress}:{port}", DateTime.Now);
                    Status.Value = ResourceStatus.Failure;

                    started = false;
                    Logger.Log(failure.Description);
                }

                //    if (TestConnection() || ipAddress.CompareTo("127.0.0.1") == 0)
                //    {
                //        await tcp.ConnectAsync(ipAddress, port);
                //        master = ModbusIpMaster.CreateIp(tcp);

                //        Status.Value = ResourceStatus.Executing;
                //        started = true;

                //        foreach (IProperty channel in channels)
                //        {
                //            if (channel is ModbusAnalogInput)
                //                await Task.Factory.StartNew((channel as ModbusAnalogInput).PollingAction);

                //            if (channel is ModbusDigitalInput)
                //                await Task.Factory.StartNew((channel as ModbusDigitalInput).PollingAction);
                //        }
                //    }
                //    else
                //    {
                //        failure = new Failure($"Unable to connect to modbus server at {ipAddress}:{port}", DateTime.Now);
                //        Status.Value = ResourceStatus.Failure;
                //    }
            }
            catch (Exception ex)
            {
                failure = new Failure(ex.Message, DateTime.Now);
                Status.Value = ResourceStatus.Failure;

                started = false;
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// Stop the <see cref="ModbusResource"/>
        /// </summary>
        public override void Stop()
        {
            Status.Value = ResourceStatus.Stopping;

            if (TestConnection() || ipAddress.CompareTo("127.0.0.1") == 0)
            {
                tcp.Close();
                Status.Value = ResourceStatus.Stopped;
            }

            if (status.Value == ResourceStatus.Failure)
                failure = new Failure("Error occurred while closing the port!", DateTime.Now);

            started = false;
        }

        private ushort[] ConvertFromDoubleToUInt16(double x)
        {
            ushort[] converted = new ushort[4];
            byte[] bytes = BitConverter.GetBytes(x);

            for (int i = 0; i < converted.Length; i++)
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
            byte[] bytes = BitConverter.GetBytes((short)x);
            ushort converted = BitConverter.ToUInt16(bytes, 0);

            return converted;
        }

        private double ConvertFromUInt16ToDouble(ushort[] values, NumericRepresentation representation)
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
                    tmp.AddRange(values.ToList());

                for (int i = 0; i < 4 - values.Length; i++)
                    tmp.Add(0);

                if (representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
                    tmp.AddRange(values.ToList());

                values = tmp.ToArray();
            }

            Buffer.BlockCopy(values, 0, bytes, 0, n + diff);

            if (representation == NumericRepresentation.Double || representation == NumericRepresentation.Single)
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