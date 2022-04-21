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
        private FuzzySystem currentSystem, speedSystem;

        private FuzzyVariable fuzzyTemperature, fuzzyPressure, fuzzyHumidity, fuzzyCurrent, fuzzySpeed;

        private FuzzyRegulator currentRegulator, speedRegulator;

        private AnalogInput temperature, pressure, humidity;
        private AnalogOutput current, speed;

        private Stopwatch t;

        [OneTimeSetUp]
        public void Setup()
        {
            currentSystem = new FuzzySystem("CurrentSystem");
            speedSystem = new FuzzySystem("SpeedSystem");

            // Input fuzzy variables
            fuzzyTemperature = new FuzzyVariable("Temperature", 0d, 100d, "°C", "0.00");
            fuzzyPressure = new FuzzyVariable("Pressure", 1d, 4d, "bar", "0.00");
            fuzzyHumidity = new FuzzyVariable("Humidity", 10d, 80d, "%", "0.00");

            // Output fuzzy variables
            fuzzyCurrent = new FuzzyVariable("Current", 0d, 6d, "A", "0.000");
            fuzzySpeed = new FuzzyVariable("Speed", 0d, 400d, "rpm", "0.0");

            // Analog inputs
            temperature = new AnalogInput("Temperature", "°C", "0.00");
            pressure = new AnalogInput("Pressure", "bar", "0.00");
            pressure.Value = 1d;
            humidity = new AnalogInput("Humidity", "%", "0.00");
            humidity.Value = 10d;

            // Analog outputs
            current = new AnalogOutput("Current", "A", "0.000");
            speed = new AnalogOutput("Speed", "rpm", "0.0");

            // Fuzzy system creation
            fuzzyTemperature.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Bad", 0d, 15d, 30d));
            fuzzyTemperature.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Low", 30d, 40d, 50d));
            fuzzyTemperature.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Good", 50d, 60d, 70d));
            fuzzyTemperature.AddLinguisticTerm(LinguisticTerm.CreateTriangular("High", 70d, 100d, 100d));

            fuzzyPressure.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Low", 1d, 1.2, 1.8, 2d));
            fuzzyPressure.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Good", 2d, 2.1, 3.9, 4d));

            fuzzyHumidity.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Low", 10d, 30d, 50d));
            fuzzyHumidity.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Good", 50d, 51d, 59d, 60d));
            fuzzyHumidity.AddLinguisticTerm(LinguisticTerm.CreateTriangular("High", 60d, 80d, 80d));

            fuzzyCurrent.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Low", 0d, 0.1, 0.9, 1d));
            fuzzyCurrent.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Medium", 1d, 1.1, 2.9, 3d));
            fuzzyCurrent.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("High", 3d, 4d, 5.9, 6d));

            fuzzySpeed.AddLinguisticTerm(LinguisticTerm.CreateTriangular("Low", 0d, 1d, 50d));
            fuzzySpeed.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("Medium", 50d, 60d, 190d, 200d));
            fuzzySpeed.AddLinguisticTerm(LinguisticTerm.CreateTrapezoidal("High", 200d, 300d, 399, 400d));

            currentSystem.AddInput(fuzzyTemperature, 20d);
            currentSystem.AddOutput(fuzzyCurrent);

            speedSystem.AddInput(fuzzyPressure, 1d);
            speedSystem.AddInput(fuzzyHumidity, 40d);
            speedSystem.AddInput(fuzzyTemperature, 20d);
            speedSystem.AddOutput(fuzzySpeed);

            currentSystem.AddRule("if (Temperature is Bad) then Current is High");
            currentSystem.AddRule("if (Temperature is Low) then Current is Medium");
            currentSystem.AddRule("if (Temperature is Good) then Current is Medium");
            currentSystem.AddRule("if (Temperature is High) then Current is Low");

            speedSystem.AddRule("if (Pressure is Good) then Speed is High");
            speedSystem.AddRule("if (Humidity is High) then Speed is High");
            speedSystem.AddRule("if (Temperature is High) then Speed is High");
            speedSystem.AddRule("if (Pressure is Low) and (Temperature is High) then Speed is Medium");
            speedSystem.AddRule("if (Humidity is Good) and (Temperature is High) then Speed is Medium");

            Dictionary<FuzzyVariable, IProperty<double>> currentVariables = new Dictionary<FuzzyVariable, IProperty<double>>()
            {
                { fuzzyTemperature, temperature }
            };
            currentRegulator = new FuzzyRegulator(
                "CurrentRegulator",
                temperature,
                60d,
                currentSystem,
                currentVariables,
                fuzzyCurrent,
                TimeSpan.FromMilliseconds(200d)
            );

            Dictionary<FuzzyVariable, IProperty<double>> speedVariables = new Dictionary<FuzzyVariable, IProperty<double>>()
            {
                { fuzzyTemperature, temperature },
                { fuzzyPressure, pressure },
                { fuzzyHumidity, humidity }
            };
            speedRegulator = new FuzzyRegulator(
                "SpeedRegulator",
                speed,
                200d,
                speedSystem,
                speedVariables,
                fuzzySpeed,
                TimeSpan.FromMilliseconds(200d)
            );

            currentRegulator.Start();
            speedRegulator.Start();

            t = Stopwatch.StartNew();
        }

        [Test]
        [TestCase(100000, false)]
        public async Task DoControl(int intervalInMilliseconds, bool printToStandardOutput)
        {
            Stopwatch sw = Stopwatch.StartNew();
            string text;
            string currentPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "test_results",
                "fuzzy_logs",
                "current_fuzzy_log.csv"
            );
            string speedPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                "test_results",
                "fuzzy_logs",
                "speed_fuzzy_log.csv"
            );

            double time;
            using (StreamWriter currentWriter = File.CreateText(currentPath))
            {
                using (StreamWriter speedWriter = File.CreateText(speedPath))
                {
                    do
                    {
                        temperature.Value = 50d + new Random(Guid.NewGuid().GetHashCode()).Next(-30, 30);
                        pressure.Value = 1d + new Random(Guid.NewGuid().GetHashCode()).Next(0, 3);
                        humidity.Value = 40d + new Random(Guid.NewGuid().GetHashCode()).Next(-30, 40);

                        time = t.Elapsed.TotalMilliseconds;
                        text = $"{time.ToString("F3").Replace(',', '.')}; " +
                            $"{temperature.Value.ToString("F3").Replace(',', '.')}; " +
                            $"{currentRegulator.Output.Value.ToString("F3").Replace(',', '.')}";

                        currentWriter.WriteLine(text);

                        text = $"{time.ToString("F3").Replace(',', '.')}; " +
                            $"{temperature.Value.ToString("F3").Replace(',', '.')}; " +
                            $"{pressure.Value.ToString("F3").Replace(',', '.')}; " +
                            $"{humidity.Value.ToString("F3").Replace(',', '.')}; " +
                            $"{speedRegulator.Output.Value.ToString("F3").Replace(',', '.')}";

                        speedWriter.WriteLine(text);

                        if (printToStandardOutput)
                            Console.WriteLine(text);

                        await Task.Delay(200);
                    } while (sw.Elapsed.TotalMilliseconds <= intervalInMilliseconds);
                }
            }
        }
    }
}