using System;
using System.Collections.Generic;
using System.IO;

namespace ArtificialIntelligence
{
    /// <summary>
    /// Implement a neural network
    /// </summary>
    public class NeuralNetwork
    {
        private int[] layers;    
        private float[][] neurons;    
        private float[][] biases;    
        private float[][][] weights;    
        private int[] activations;

        private float fitness;

        /// <summary>
        /// The fitness
        /// </summary>
        public float Fitness { get => fitness; set => fitness = value; }

        /// <summary>
        /// Create a new instance of <see cref="NeuralNetwork"/>
        /// </summary>
        /// <param name="layers">The network layer</param>
        public NeuralNetwork(int[] layers)
        {
            this.layers = layers;

            InitializeNeurons();
            InitializeBiases();
            InitializeWeights();
        }

        /// <summary>
        /// Create an empty storage for the neurons in the network
        /// </summary>
        private void InitializeNeurons()
        {
            List<float[]> neuronsList = new List<float[]>();
            for (int i = 0; i < layers.Length; i++)
                neuronsList.Add(new float[layers[i]]);

            neurons = neuronsList.ToArray();
        }

        /// <summary>
        /// Initialize and populate the biases array being held within the network
        /// </summary>
        private void InitializeBiases()
        {
            List<float[]> biasList = new List<float[]>();
            float[] bias;

            for (int i = 0; i < layers.Length; i++)
            {
                bias = new float[layers[i]];
                for (int j = 0; j < layers[i]; j++)
                    bias[j] = GenerateRandomNumber();

                biasList.Add(bias);
            }

            biases = biasList.ToArray();
        }

        /// <summary>
        /// Initialize a random array for the weights being held in the network
        /// </summary>
        private void InitializeWeights()
        {
            List<float[][]> weightsList = new List<float[][]>();
            List<float[]> layerWeightsList;
            float[] neuronWeights;
            int neuronsInPreviousLayer;

            for (int i = 1; i < layers.Length; i++)
            {
                layerWeightsList = new List<float[]>();
                neuronsInPreviousLayer = layers[i - 1];

                for (int j = 0; j < neurons[i].Length; j++)
                {
                    neuronWeights = new float[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                        neuronWeights[k] = GenerateRandomNumber();

                    layerWeightsList.Add(neuronWeights);
                }

                weightsList.Add(layerWeightsList.ToArray());
            }

            weights = weightsList.ToArray();
        }

        /// <summary>
        /// Generate a random number between -0.5 and +0.5
        /// </summary>
        /// <returns>The random number generated</returns>
        private float GenerateRandomNumber()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());

            int min = -500, max = 501;
            float value = rnd.Next(min, max) / 1000f;

            return value;
        }

        /// <summary>
        /// Generate a random number between 0 and <paramref name="max"/>
        /// </summary>
        /// <param name="max">The maximum value</param>
        /// <returns>The random number generated</returns>
        private float GenerateRandomNumber(int max)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            float value = (float)(max * rnd.NextDouble());

            return value;
        }

        /// <summary>
        /// Generate  a random number between -<paramref name="limit"/> and
        /// <paramref name="limit"/>
        /// </summary>
        /// <param name="limit">The range limit</param>
        /// <returns>The random number generated</returns>
        private float GenerateRandomNumber(float limit)
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            int sign = rnd.Next(-1, 1);

            float value = (float)(sign * limit * rnd.NextDouble());
            return value;
        }

        /// <summary>
        /// Activation function
        /// </summary>
        /// <param name="value">The activation function input</param>
        /// <returns>The activation function output</returns>
        public float Activate(float value) => (float)Math.Tanh(value);

        /// <summary>
        /// Feed forward the inputs
        /// </summary>
        /// <param name="inputs">The network inputs</param>
        /// <returns>The network outputs</returns>
        public float[] FeedForward(float[] inputs)
        {
            for (int i = 0; i < inputs.Length; i++)
                neurons[0][i] = inputs[i];

            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    float value = 0f;
                    for (int k = 0; k < neurons[i - 1].Length; k++)
                        value += weights[i - 1][j][k] * neurons[i - 1][k];

                    neurons[i][j] = Activate(value + biases[i][j]);
                }
            }

            return neurons[neurons.Length - 1];
        }

        /// <summary>
        /// Compare <see cref="NeuralNetwork"/> performances
        /// </summary>
        /// <param name="nn">The other <see cref="NeuralNetwork"/></param>
        /// <returns>The comparison output</returns>
        public int CompareTo(NeuralNetwork nn)
        {
            int output = 0;

            if (nn == null || fitness > nn.fitness)
                output = 1;
            else 
                if (fitness < nn.fitness)
                    output = -1;

            return output;
        }

        /// <summary>
        /// Load the weights and biases from a file int the network
        /// </summary>
        /// <param name="path">The file path</param>
        public void LoadFromPath(string path)
        {
            TextReader tr = new StreamReader(path);
            int numberOfLines = (int)new FileInfo(path).Length;
            string[] listLines = new string[numberOfLines];
            int index = 1;

            for (int i = 1; i < numberOfLines; i++)
                listLines[i] = tr.ReadLine();

            tr.Close();

            if (new FileInfo(path).Length > 0)
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = float.Parse(listLines[index]);
                        index++;
                    }
                }
                for (int i = 0; i < weights.Length; i++)
                {
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            weights[i][j][k] = float.Parse(listLines[index]);
                            index++;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Simple mutation function for any genetic implementation
        /// </summary>
        /// <param name="chance">The chance</param>
        /// <param name="value">The value</param>
        public void Mutate(int chance, float value)
        {
            for (int i = 0; i < biases.Length; i++)
                for (int j = 0; j < biases[i].Length; j++)
                    biases[i][j] = (GenerateRandomNumber(chance) <= 5) ? biases[i][j] += GenerateRandomNumber(value) : biases[i][j];

            for (int i = 0; i < weights.Length; i++)
                for (int j = 0; j < weights[i].Length; j++)
                    for (int k = 0; k < weights[i][j].Length; k++)
                        weights[i][j][k] = (GenerateRandomNumber(chance) <= 5) ? weights[i][j][k] += GenerateRandomNumber(value): weights[i][j][k];
        }
    }
}
