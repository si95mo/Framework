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
        private int firstDataBlock = 2, secondDataBlock = 3;

        private Snap7AnalogInput firstAnalogInput, secondAnalogInput;
        private Snap7AnalogOutput firstAnalogOutput, secondAnalogOutput;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new Snap7Resource("Snap7Resource", ipAddress, rack: 0, slot: 0, pollingInterval: 100);
            resource.AddDataBlock(dataBlock: 2, size: 256);
            resource.AddDataBlock(dataBlock: 3, size: 256);

            firstAnalogInput = new Snap7AnalogInput(
                "FirstAnalogInput",
                memoryAddress: 0,
                dataBlock: firstDataBlock,
                resource,
                RepresentationBytes.Two,
                NumericRepresentation.Int16,
                format: "0"
            ); 
            secondAnalogInput = new Snap7AnalogInput(
                 "SecondAnalogInput",
                 memoryAddress: 0,
                 dataBlock: secondDataBlock,
                 resource,
                 RepresentationBytes.Two,
                 NumericRepresentation.Int16,
                format: "0"
             );

            firstAnalogOutput = new Snap7AnalogOutput(
                "FirstAnalogOutput",
                memoryAddress: 0,
                dataBlock: firstDataBlock,
                resource,
                RepresentationBytes.Two,
                NumericRepresentation.Int16,
                format: "0"
            );
            secondAnalogOutput = new Snap7AnalogOutput(
                "SecondAnalogOutput",
                memoryAddress: 0,
                dataBlock: secondDataBlock,
                resource,
                RepresentationBytes.Two,
                NumericRepresentation.Int16,
                format: "0"
            );

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);
        }

        [Test]
        public async Task Test()
        {
            int counter = 0;
            List<double> firstInputs = new List<double>();
            List<double> secondInputs = new List<double>();

            Stopwatch sw = Stopwatch.StartNew();

            do
            {
                firstAnalogOutput.Value = counter++;
                secondAnalogOutput.Value = 2 * counter;
                await Task.Delay(1000);

                firstInputs.Add(firstAnalogInput.Value);
                secondInputs.Add(secondAnalogInput.Value);

                Console.WriteLine($"{DateTime.Now:HH.mm.ss.fff} >> 1st out: {firstAnalogOutput}; 1st in: {firstAnalogInput}");
                Console.WriteLine($"{DateTime.Now:HH.mm.ss.fff} >> 2nd out: {secondAnalogOutput}; 2nd in: {secondAnalogInput}");
            } while (sw.Elapsed.TotalMilliseconds <= 10000); // 10 seconds

            sw.Stop();

            firstInputs.Distinct().Count().Should().BeInRange(firstInputs.Count - 2, firstInputs.Count);
            secondInputs.Distinct().Count().Should().BeInRange(secondInputs.Count - 2, secondInputs.Count);
        }
    }
}