using Hardware;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Control.PID.Tests
{
    public class TestClass
    {
        private double r = 1.0; // 1 Ohm
        private double c = 1.0; // 1 F
        private Stopwatch t;

        private PID pid;

        private AnalogInput u;

        private double CalculateVoltage()
        {
            double vc = pid.Uk.Value * (1 - Math.Exp(-t.Elapsed.TotalSeconds / (r * c)));
            return vc;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            t = Stopwatch.StartNew();

            u = new AnalogInput("U", "V", "0.000");
            pid = new PID(
                code: "PID",
                feedback: u,
                n: 100,
                kp: 0.01,
                ki: 4,
                kd: 0.0001,
                upperLimit: 10,
                lowerLimit: 0,
                setpoint: 5.0
            );

            pid.Start();
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
                "pid_logs",
                "pid_log.csv"
            );

            double time;
            using (StreamWriter writer = File.CreateText(path))
            {
                do
                {
                    u.Value = CalculateVoltage();

                    await Task.Delay(10);

                    time = t.Elapsed.TotalMilliseconds;
                    text = $"{time.ToString("F3").Replace(',', '.')}; " +
                        $"{pid.Setpoint.ToString("F3").Replace(',', '.')}; " +
                        $"{pid.Rk.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{pid.Uk.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{u.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    if (time >= 33333 && time <= 66666)
                        pid.Setpoint = 3.3;
                    else
                    {
                        if (time >= 66000)
                            pid.Setpoint = 10.0;
                    }
                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}