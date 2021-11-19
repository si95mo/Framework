using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace ArtificialIntelligence.Tests
{
    [TestFixture]
    public class NeuralNetworkTestClass
    {
        private double[][] inputs = {
            new[] {0.0, 0.0},
            new[] {1.0, 0.0},
            new[] {0.0, 1.0},
            new[] {1.0, 1.0}
        };

        private double[][] outputs = {
            new[] {0.0},
            new[] {1.0},
            new[] {1.0},
            new[] {0.0}
        };

        //public List<Bot> Bots;
        public NeuralNetwork nn;

        [OneTimeSetUp]
        public void Setup()
        {
            nn = new NeuralNetwork(inputs, outputs);

            nn.AddLayer(2, true, ActivationFunction.None);
            nn.AddLayer(3, true, ActivationFunction.Sigmoid);
            nn.AddLayer(1, false, ActivationFunction.Sigmoid);

            nn.UpdateNetwork();
        }

        [Test]
        [TestCase(0.01)]
        [TestCase(0.001)]
        [TestCase(0.0001)]
        [TestCase(0.00001)]
        [TestCase(0.000001)]
        public void TrainNetwork(double errorThreshold)
        {
            nn.Train(errorThreshold);
            nn.Error.Should().BeLessThanOrEqualTo(errorThreshold);

            List<(double[] Inputs, double[] Outputs, double[] Ideals)> result = nn.Test();
            Console.WriteLine(GetReadableString(result, errorThreshold, nn.EpochCount));
        }

        public string GetReadableString(List<(double[] Inputs, double[] Outputs, double[] Ideals)> data, double errorThreshold, int epochCount)
        {
            string str = $"Error: {errorThreshold:0.000000}, epoch count: {epochCount}{Environment.NewLine}";
            int index = 0;

            foreach ((double[] Inputs, double[] Outputs, double[] Ideals) item in data)
            {
                str += $"Inputs [{++index}]:";
                for (int i = 0; i < item.Inputs.Length; i++)
                    str += $" {item.Inputs[i]:0.000000}";

                str += $". Outputs [{index}]:";
                for (int i = 0; i < item.Outputs.Length; i++)
                    str += $" {item.Outputs[i]:0.000000}";

                str += $". Ideals [{index}]:";
                for (int i = 0; i < item.Ideals.Length; i++)
                    str += $" {item.Ideals[i]:0.000000}";

                str += Environment.NewLine;
            }

            return str;
        }
    }
}