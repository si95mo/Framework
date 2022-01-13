using Core;
using FluentAssertions;
using Hardware.Snap7;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class Snap7ResourceTestClass
    {
        private Snap7Resource resource;
        private string ipAddress = "127.0.0.1";
        private int dataBlock = 2;

        private Snap7AnalogInput analogInput;
        private Snap7AnalogOutput analogOutput;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new Snap7Resource("Snap7Resource", ipAddress, rack: 0, slot: 0, pollingInterval: 100);
            resource.AddDataBlock(dataBlock: 2, size: 256);

            analogInput = new Snap7AnalogInput(
                "Snap7AnalogInput",
                memoryAddress: 0,
                dataBlock: dataBlock,
                resource,
                RepresentationBytes.Two,
                NumericRepresentation.Int16
            );
            analogOutput = new Snap7AnalogOutput(
                "Snap7AnalogOutput",
                memoryAddress: 0,
                dataBlock: dataBlock,
                resource,
                RepresentationBytes.Two,
                NumericRepresentation.Int16
            );

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [Test]
        public async Task Test()
        {
            int counter = 0;
            List<double> inputs = new List<double>();

            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                analogOutput.Value = counter++;
                await Task.Delay(1000);

                inputs.Add(analogInput.Value);

                Console.WriteLine($"{DateTime.Now:HH.mm.ss.fff} >> Out: {analogOutput}; In: {analogInput}");
            } while (sw.Elapsed.TotalMilliseconds <= 10000); // 10 seconds

            sw.Stop();

            inputs.Distinct().Count().Should().BeInRange(inputs.Count - 2, inputs.Count);
        }
    }
}