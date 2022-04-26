using Hardware;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Control.Pwm.Tests
{
    [TestFixture]
    public class TestClass
    {
        private double r = 100; // 0.1 kOhm
        private double c = 0.01; // 10 mF

        private Stopwatch t;

        private PwmRegulator regulator;

        private AnalogInput u; // Feedback
        private DigitalOutput m; // Actuator

        private double CalculateVoltageIncrement()
        {
            double vc = 24 * (1 - Math.Exp(-regulator.Ton.Value / 1000 / (r * c))); // 24V
            return vc;
        }

        private double CalculateVoltageDecrement()
        {
            double vc = -u.Value * (1 - Math.Exp(-(regulator.CycleTime.ValueInMilliseconds - regulator.Ton.Value) / 1000 / (r * c)));
            return vc;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            t = Stopwatch.StartNew();

            u = new AnalogInput("U", "V", "0.000");
            m = new DigitalOutput("M");

            regulator = new PwmRegulator(
                code: "Regulator",
                feedbackChannel: u,
                actuatorChannel: m,
                maximumPercentage: 100d,
                minimumPercentage: 0d,
                n: 10,
                kp: 1.6,
                ki: 4,
                kd: 0,
                setpoint: 8d,
                cycleTime: 250
            );

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
                "pwm_logs",
                "pwm_log.csv"
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
                        $"{regulator.Feedback.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.PwmPercentage.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    if (time >= intervalInMilliseconds / 3 && time <= 2 * intervalInMilliseconds / 3)
                        regulator.Setpoint.Value = 5d;
                    else
                    {
                        if (time >= 2 * intervalInMilliseconds / 3)
                            regulator.Setpoint.Value = 6d;
                    }

                    await Task.Delay(regulator.CycleTime.Value);

                    u.Value += CalculateVoltageDecrement();
                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}