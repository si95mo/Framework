﻿using FluentAssertions;
using Hardware.Twincat;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    [TestFixture]
    public class TwincatResourceTestClass
    {
        private readonly string amsNetAddress = "169.254.174.61.1.1"; // Check which is correct for the local ads server
        private readonly int port = 851; // TwinCAT - local ads server

        private TwincatResource resource;


        private TwincatAnalogInput analogIn;
        private TwincatDigitalInput digitalIn;
        private TwincatAnalogOutput analogOut;
        private TwincatDigitalOutput digitalOut;

        [OneTimeSetUp]
        public async Task Setup()
        {
            resource = new TwincatResource("TwincatResource", amsNetAddress, port);

            await resource.Start();
            resource.Status.Value.Should().Be(ResourceStatus.Executing);

            analogIn = new TwincatAnalogInput("AnalogInputVariableName", "GVL.AIn[0]", resource);
            digitalIn = new TwincatDigitalInput("DigitalInputVariableName", "GVL.DIn[0]", resource);
            analogOut = new TwincatAnalogOutput("AnalogOutputVariableName", "GVL.AOut[0]", resource);
            digitalOut = new TwincatDigitalOutput("DigitalOutputVariableName", "GVL.DOUt[0]", resource);
        }

        [Test]
        public async Task Test()
        {
            for(int i = 1; i <= 10; i++)
            {
                analogOut.Value = i;
                digitalOut.Value = !digitalOut.Value;

                await Task.Delay(1000);

                Console.WriteLine($"{DateTime.Now:HH:mm:ss.fff} >> {analogIn.VariableName}: {analogIn.Value}, {digitalIn.VariableName}: {digitalIn.Value}");
            }
        }
    }
}