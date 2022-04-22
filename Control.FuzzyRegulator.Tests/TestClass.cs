using Core;
using Hardware;
using Mathematics.FuzzyLogic;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Control.FuzzyRegulator.Tests
{
    [TestFixture]
    public class TestClass
    {
        private double r = 100; // 0.1 kOhm
        private double c = 0.01; // 10 mF

        private FuzzySystem fuzzySystem;
        private FuzzyVariable fuzzyVoltage, fuzzyOutput;
        private FuzzyRegulator regulator;

        private AnalogInput u;
        private AnalogOutput vc;

        private Stopwatch t;

        [OneTimeSetUp]
        public void Setup()
        {
            fuzzySystem = new FuzzySystem("FuzzySystem");

            // Input fuzzy variables
            fuzzyVoltage = new FuzzyVariable("Voltage", 0d, 24d, "V", "0.000");

            // Output fuzzy variables
            fuzzyOutput = new FuzzyVariable("Output", 0d, 24d, "V", "0.000");

            // Analog inputs
            u = new AnalogInput("AiVoltage", "V", "0.000");

            // Analog outputs
            vc = new AnalogOutput("AoVoltage", "V", "0.000");

            // Fuzzy system creation
            fuzzyVoltage.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Bad", 0d, 0d, 6d));
            fuzzyVoltage.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Low", 6d, 6d, 11.8));
            fuzzyVoltage.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Good", 11.8, 11.9d, 12.1, 12.2));
            fuzzyVoltage.AddLinguisticTerm(LinguisticTerm.CreateTriangular("High", 12.2, 24d, 24d));

            fuzzyOutput.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Bad", 0d, 0d, 6d));
            fuzzyOutput.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Low", 6d, 6d, 11.8));
            fuzzyOutput.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Good", 11.8, 11.9d, 12.1, 12.2));
            fuzzyOutput.AddLinguisticTerm(LinguisticTerm.CreateTriangular("High", 12.2, 24d, 24d));

            fuzzySystem.AddInput(fuzzyVoltage, 0d);
            fuzzySystem.AddOutput(fuzzyOutput);

            fuzzySystem.AddRule("if (Voltage is Bad) then Output is High");
            fuzzySystem.AddRule("if (Voltage is Low) then Output is Good");
            fuzzySystem.AddRule("if (Voltage is Good) then Output is Good");
            fuzzySystem.AddRule("if (Voltage is High) then Output is Low");

            Dictionary<FuzzyVariable, IProperty<double>> currentVariables = new Dictionary<FuzzyVariable, IProperty<double>>()
            {
                { fuzzyVoltage, u }
            };
            regulator = new FuzzyRegulator(
                "Regulator",
                u,
                12d,
                fuzzySystem,
                currentVariables,
                fuzzyOutput,
                TimeSpan.FromMilliseconds(200d)
            );

            regulator.Output.ConnectTo(vc);
            regulator.Start();

            t = Stopwatch.StartNew();
        }
        private double CalculateVoltage()
        {
            double vc;
            if (regulator.Output.Value > u.Value) // Increment
                vc = regulator.Output.Value * (1 - Math.Exp(-regulator.CycleTime.ValueAsMilliseconds / 1000 / (r * c)));
            else // Decrement
                vc = -regulator.Output.Value * (1 - Math.Exp(-regulator.CycleTime.ValueAsMilliseconds / 1000 / (r * c)));

            return vc;
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
                "fuzzy_logs",
                "fuzzy_log.csv"
            );
            double time;

            using (StreamWriter writer = File.CreateText(path))
            {
                do
                {
                    u.Value += CalculateVoltage();

                    time = t.Elapsed.TotalMilliseconds;
                    text = $"{time.ToString("F3").Replace(',', '.')}; " +
                        $"{u.Value.ToString("F3").Replace(',', '.')}; " +
                        $"{regulator.Output.Value.ToString("F3").Replace(',', '.')}";

                    writer.WriteLine(text);

                    if (printToStandardOutput)
                        Console.WriteLine(text);

                    await Task.Delay(200);
                } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
            }
        }
    }
}