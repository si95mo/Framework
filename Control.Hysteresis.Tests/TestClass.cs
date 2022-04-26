using Hardware;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

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

        private double CalculateVoltageIncrement()
        {
            double vc = regulator.Actuator.Value ? 24 * (1 - Math.Exp(-regulator.CycleTime.ValueInMilliseconds / 1000 / (r * c))) : 0d;
            return vc;
        }

        private double CalculateVoltageDecrement()
        {
            double vc = !regulator.Actuator.Value ? -u.Value * (1 - Math.Exp(-regulator.CycleTime.ValueInMilliseconds / 2000 / (r * c))) : 0d;
            return vc;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            t = Stopwatch.StartNew();

            u = new AnalogInput("U", "V", "0.000");
            actuator = new DigitalOutput("M");

            regulator = new HysteresisRegulator(code: "Regulator", feedbackChannel: u, actuatorChannel: actuator, 12.05, 11.95, 12d, 10d);

            regulator.Start();
        }

        [Test]
        [TestCase(100000, false)]
        public async Task DoControl(int intervalInMilliseconds, bool printToStandardOutput)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string text;
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "test_results",
                "hysteresis_logs",
                "hysteresis_log.csv"
            );

            double time;
            using (StreamWriter writer = File.CreateText(path))
            {
                do
                {
                    u.Value += CalculateVoltageIncrement();

                    time = t.Elapsed.TotalMilliseconds;
                    text = $"{time.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Setpoint.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Feedback.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    if (time >= intervalInMilliseconds / 3 && time <= 2 * intervalInMilliseconds / 3)
                    {
                        regulator.Setpoint.Value = 3.3;
                        regulator.UpperLimit.Value = 3.35;
                        regulator.LowerLimit.Value = 3.25;
                    }
                    else
                    {
                        if (time >= 2 * intervalInMilliseconds / 3)
                        {
                            regulator.Setpoint.Value = 5d;
                            regulator.UpperLimit.Value = 5.05;
                            regulator.LowerLimit.Value = 4.95;
                        }
                    }

                    await Task.Delay(regulator.CycleTime.Value);

                    u.Value += CalculateVoltageDecrement();
                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}