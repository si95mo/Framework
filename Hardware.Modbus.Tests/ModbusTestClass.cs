using Core;
using Core.DataStructures;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Hardware.Modbus.Tests
{
    public class ModbusTestClass
    {
        private ModbusResource resource;

        private ModbusAnalogOutput doubleAnalogOut;
        private ModbusAnalogInput  doubleAnalogIn;
        private ModbusAnalogOutput singleAnalogOut;
        private ModbusAnalogInput  singleAnalogIn;
        private ModbusAnalogOutput intAnalogOut;
        private ModbusAnalogInput  intAnalogIn;
        private ModbusAnalogOutput uintAnalogOut;
        private ModbusAnalogInput  uintAnalogIn;

        private ModbusDigitalOutput digitalOut;
        private ModbusDigitalInput digitalIn;

        [OneTimeSetUp]
        public void Setup()
        {
            resource = new ModbusResource("ModbusResource", "127.0.0.1", 502);

            doubleAnalogOut = new ModbusAnalogOutput("ModbusDoubleAnalogOutput", resource, 0, representation: NumericRepresentation.Double);
            doubleAnalogIn = new ModbusAnalogInput(  "ModbusDoubleAnalogInput",  resource, 0, representation: NumericRepresentation.Double);

            singleAnalogIn = new ModbusAnalogInput("ModbusSingleAnalogInput", resource, 10, representation: NumericRepresentation.Single);
            singleAnalogOut = new ModbusAnalogOutput("ModbusSingleAnalogOutput", resource, 10, representation: NumericRepresentation.Single);

            intAnalogOut = new ModbusAnalogOutput("ModbusIntAnalogOutput", resource, 20, representation: NumericRepresentation.Int32);
            intAnalogIn = new ModbusAnalogInput("ModbusIntAnalogInput", resource, 20, representation: NumericRepresentation.Int32);

            uintAnalogOut = new ModbusAnalogOutput("ModbusUIntAnalogOutput", resource, 30, representation: NumericRepresentation.UInt16);
            uintAnalogIn = new ModbusAnalogInput("ModbusUIntAnalogInput", resource, 30, representation: NumericRepresentation.UInt16);

            digitalOut = new ModbusDigitalOutput("ModbusDigitalOutput", resource, 0);
            digitalIn = new ModbusDigitalInput("ModbusDigitalInput", resource, 0);

            ServiceBroker.Init();

            ServiceBroker.Add<IResource>(resource);

            ServiceBroker.Add<IModbusChannel>(doubleAnalogOut);
            ServiceBroker.Add<IModbusChannel>(doubleAnalogIn);

            ServiceBroker.Add<IModbusChannel>(singleAnalogOut);
            ServiceBroker.Add<IModbusChannel>(singleAnalogIn);

            ServiceBroker.Add<IModbusChannel>(intAnalogOut);
            ServiceBroker.Add<IModbusChannel>(intAnalogIn);

            ServiceBroker.Add<IModbusChannel>(uintAnalogOut);
            ServiceBroker.Add<IModbusChannel>(uintAnalogIn);

            ServiceBroker.Add<IModbusChannel>(digitalOut);
            ServiceBroker.Add<IModbusChannel>(digitalIn);

            ServiceBroker.Get<IResource>().Should().NotBeNull();
            ServiceBroker.Get<IModbusChannel>().Should().NotBeNull();

            resource.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            ServiceBroker.Clear();
            resource.Stop();
        }

        [Test]
        public void WriteAndRead()
        {
            doubleAnalogOut.Value = 1.0;
            singleAnalogOut.Value = 2.0;
            intAnalogOut.Value = 3;
            uintAnalogOut.Value = 4;
            digitalOut.Value = true;

            Task.Delay(1000).Wait();
            doubleAnalogIn.Value.Should().Be(doubleAnalogOut.Value);

            Task.Delay(1000).Wait();
            singleAnalogIn.Value.Should().Be(singleAnalogOut.Value);
            
            Task.Delay(1000).Wait();
            intAnalogIn.Value.Should().Be(intAnalogOut.Value);
            
            Task.Delay(1000).Wait();
            uintAnalogIn.Value.Should().Be(uintAnalogOut.Value);

            Task.Delay(1000).Wait();
            digitalIn.Value.Should().BeTrue();
        }
    }
}
