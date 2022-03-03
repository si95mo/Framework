using Hardware;
using NUnit.Framework;
using System;
using System.Diagnostics;

namespace Control.Hysteresis.Tests
{
    [TestFixture]
    public class TestClass
    {
        private double r = 1.0; // 1 Ohm
        private double c = 1.0; // 1 F
        private Stopwatch t;

        private HysteresisRegulator regulator;

        private AnalogInput u;
        private DigitalOutput actuator;

        private double CalculateVoltage()
        {
            double vc = regulator.Output.Value * (1 - Math.Exp(-t.Elapsed.TotalSeconds / (r * c)));
            return vc;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            t = Stopwatch.StartNew();

            u = new AnalogInput("U", "V", "0.000");
            regulator = new HysteresisRegulator(code: "Regulator", feedbackChannel: u, actuatorChannel: actuator, 10.5, 9.5, 10d, 10d);

            regulator.Start();
        }
    }
}