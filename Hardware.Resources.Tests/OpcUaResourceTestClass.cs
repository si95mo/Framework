using FluentAssertions;
using Hardware.Opc.Ua;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    public class OpcUaResourceTestClass
    {
        private OpcUaResource resource;
        private OpcUaInput temperature;
        private OpcUaInput pressure;

        private OpcUaOutput output;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource    = new OpcUaResource("OpcUaResource", "opc.tcp://localhost:4840/");

            temperature = new OpcUaInput("AiTemperature", "ns=2;s=Temperature", resource, "°C");
            pressure    = new OpcUaInput("AiPressure",    "ns=2;s=Pressure",    resource, "Pa");

            output = new OpcUaOutput("AoTemperature", "ns=2;s=Temperature", resource, "°C");

            await resource.Start();
            resource.Status.Should().Be(ResourceStatus.Executing);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            resource.Stop();
        }

        [Test]
        public async Task Test()
        {
            output.Value = 10;
            Stopwatch sw = Stopwatch.StartNew();

            await Task.Delay(1000);

            do
            {
                temperature.Value.Should().NotBe(0.0);
                pressure.Value.Should().NotBe(0.0);

                Console.WriteLine(
                    $"{DateTime.Now:HH:mm:ss} >> Temperature: {temperature}; pressure: {pressure}"
                );

                await Task.Delay(1000);
            } while (sw.Elapsed.TotalMilliseconds <= 10000);
        }
    }
}