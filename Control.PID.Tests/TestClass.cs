﻿using FluentAssertions;
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

        PID pid;

        AnalogInput u;

        private double CalculateVoltage()
        {
            double vc = pid.Output.Value * (1 - Math.Exp(-t.Elapsed.TotalSeconds / (r * c)));
            return vc;
        }

        [OneTimeSetUp]
        public void Setup()
        {
            t = Stopwatch.StartNew();

            u = new AnalogInput("U", "V", "0.000");
            pid = new PID(
                code: "PID", 
                u: u, 
                kp: 0.0001, 
                ki: 8, 
                kd: 0.00001, 
                upperLimit: 10, 
                lowerLimit: 0, 
                setPoint: 5.0
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
                        $"{pid.SetPoint.ToString("F3").Replace(',', '.')}; " +
                        $"{pid.Output.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{u.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    if (time >= 33000 && time <= 66000)
                        pid.SetPoint = 3.3;
                    else
                    {
                        if (time >= 66000)
                            pid.SetPoint = 10.0;
                    }
                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}
