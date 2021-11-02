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

        private OpcUaAnalogInput AiTemperature;
        private OpcUaAnalogInput AiPressure;
        private OpcUaDigitalInput DiTrigger;

        private OpcUaAnalogOutput AoTemperature;
        private OpcUaDigitalOutput DoTrigger;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new OpcUaResource("OpcUaResource", "opc.tcp://localhost:4840/");

            AiTemperature = new OpcUaAnalogInput(nameof(AiTemperature), "ns=2;s=Temperature", resource, "°C");
            AiPressure = new OpcUaAnalogInput(nameof(AiPressure), "ns=2;s=Pressure", resource, "Pa");
            DiTrigger = new OpcUaDigitalInput(nameof(DiTrigger), "ns=2;s=Trigger", resource);

            AoTemperature = new OpcUaAnalogOutput(nameof(AoTemperature), "ns=2;s=Temperature", resource, "°C");
            DoTrigger = new OpcUaDigitalOutput(nameof(DoTrigger), "ns=2;s=Trigger", resource);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            resource.Stop();
        }

        [Test]
        public async Task Test()
        {
            AoTemperature.Value = 10;
            Stopwatch sw = Stopwatch.StartNew();

            await Task.Delay(1000);

            AiTemperature.Value.Should().BeApproximately(AoTemperature.Value, 2d);

            do
            {
                AiTemperature.Value.Should().NotBe(0.0);
                AiPressure.Value.Should().NotBe(0.0);

                DoTrigger.Value = !DoTrigger.Value;
                await Task.Delay(200);
                DiTrigger.Value.Should().Be(DoTrigger.Value);

                Console.WriteLine(
                    $"{DateTime.Now:HH:mm:ss} >> Temperature: {AiTemperature}; pressure: {AiPressure}; trigger: {DiTrigger}"
                );

                await Task.Delay(800);
            } while (sw.Elapsed.TotalMilliseconds <= 10000);
        }
    }
}