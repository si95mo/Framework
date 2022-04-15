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
        private double r = 1.0; // 1 Ohm
        private double c = 1.0; // 1 F

        private Stopwatch t;

        private PwmRegulator regulator;

        private AnalogInput u; // Feedback
        private DigitalOutput m; // Actuator

        private double CalculateTemperatureIncrement()
        {
            double vc = regulator.Output.Value * (1 - Math.Exp(regulator.Ton.Value / (r * c)));
            return vc;
        }

        private double CalculateTemperatureDecrement()
        {
            double vc = regulator.Output.Value * (1 - Math.Exp((regulator.CycleTime.ValueAsMilliseconds - regulator.Ton.Value) / (r * c)));
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
                kp: 1,
                ki: 0,
                kd: 0,
                setpoint: 80d,
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
                double oldTemperature = 20d;
                do
                {
                    u.Value += CalculateTemperatureIncrement();
                    oldTemperature = u.Value;

                    time = t.Elapsed.TotalMilliseconds;
                    text = $"{time.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Setpoint.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Feedback.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Output.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.PwmPercentage.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    if (time >= intervalInMilliseconds / 3 && time <= 2 * intervalInMilliseconds / 3)
                        regulator.Setpoint.Value = 50d;
                    else
                    {
                        if (time >= 2 * intervalInMilliseconds / 3)
                            regulator.Setpoint.Value = 60d;
                    }

                    int tOff = (int)(regulator.CycleTime.Value.TotalMilliseconds - regulator.Ton.Value);
                    await Task.Delay(tOff);

                    u.Value -= CalculateTemperatureDecrement();
                    oldTemperature = u.Value;

                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}
