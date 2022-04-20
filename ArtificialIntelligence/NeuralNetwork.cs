using Encog.Engine.Network.Activation;
using Encog.ML.Data;
using Encog.ML.Data.Basic;
using Encog.ML.Train;
using Encog.Neural.Networks;
using Encog.Neural.Networks.Layers;
using Encog.Neural.Networks.Training.Propagation.Resilient;
using System.Collections.Generic;

namespace ArtificialIntelligence
{
    /// <summary>
    /// Define the <see cref="NeuralNetwork"/> activation function
    /// </summary>
    public enum ActivationFunction
    {
        /// <summary>
        /// A non activation function
        /// </summary>
        None = 0,

        /// <summary>
        /// A sigmoid-type function
        /// </summary>
        Sigmoid = 1,

        /// <summary>
        /// A linear-type function
        /// </summary>
        Linear = 2,

        /// <summary>
        /// A Gaussian-type function
        /// </summary>
        Gaussian = 3
    }

    /// <summary>
    /// Implement a neural network
    /// </summary>
    public class NeuralNetwork
    {
        private double[][] inputs;
        private double[][] outputs;

        private int layerCount;
        private int epochCount;

        private BasicNetwork nn;

        private IMLDataSet trainingSet;
        private IMLTrain train;

        /// <summary>
        /// The network inputs
        /// </summary>
        public double[][] Inputs { get => inputs; set => inputs = value; }

        /// <summary>
        /// The network outputs
        /// </summary>
        public double[][] Outputs { get => outputs; }

        /// <summary>
        /// The network train error
        /// </summary>
        public double Error => train.Error;

        /// <summary>
        /// The network layer counter
        /// </summary>
        public int LayerCount => layerCount;

        /// <summary>
        /// The network epoch counter
        /// </summary>
        public int EpochCount => epochCount;

        /// <summary>
        /// Create a new instance of <see cref="NeuralNetwork"/>
        /// </summary>
        public NeuralNetwork()
        {
            nn = new BasicNetwork();
        }

        /// <summary>
        /// Create a new instance of <see cref="NeuralNetwork"/>
        /// </summary>
        /// <param name="inputs">The network inputs</param>
        /// <param name="outputs">The network outputs</param>
        public NeuralNetwork(double[][] inputs, double[][] outputs) : this()
        {
            this.inputs = inputs;
            this.outputs = outputs;

            layerCount = 0;
            epochCount = 0;

            trainingSet = new BasicMLDataSet(inputs, outputs);
        }

        /// <summary>
        /// Update the network data set
        /// </summary>
        public void UpdateDataSet()
            => trainingSet = new BasicMLDataSet(inputs, outputs);

        /// <summary>
        /// Add a layer to the network
        /// </summary>
        /// <param name="neuronCounter">The layer neuron counter</param>
        /// <param name="hasBias">The bias option</param>
        /// <param name="activationFunction">The <see cref="ActivationFunction"/></param>
        public void AddLayer(int neuronCounter, bool hasBias, ActivationFunction activationFunction = ActivationFunction.Sigmoid)
        {
            IActivationFunction af = null; // ActivationFunction.None
            switch (activationFunction)
            {
                case ActivationFunction.Sigmoid:
                    af = new ActivationSigmoid();
                    break;

                case ActivationFunction.Linear:
                    af = new ActivationLinear();
                    break;

                case ActivationFunction.Gaussian:
                    af = new ActivationGaussian();
                    break;
            }

            nn.AddLayer(new BasicLayer(af, hasBias, neuronCounter));
            layerCount++;
        }

        /// <summary>
        /// Update the network structure
        /// </summary>
        /// <remarks>
        /// This method should be called after <see cref="AddLayer(int, bool, ActivationFunction)"/>
        /// and only if the network has at least two <see cref="LayerCount"/> and before calling
        /// <see cref="Train(double)"/>!
        /// </remarks>
        public void UpdateNetwork()
        {
            if (layerCount >= 2)
            {
                nn.Structure.FinalizeStructure();
                nn.Reset();
            }
        }

        /// <summary>
        /// Train the network
        /// </summary>
        /// <param name="errorThreshold">The error threshold to achieve</param>
        public void Train(double errorThreshold = 0.01)
        {
            train = new ResilientPropagation(nn, trainingSet);

            epochCount = 1;
            do
            {
                train.Iteration();
                epochCount++;
            } while (train.Error > errorThreshold);
        }

        /// <summary>
        /// Test the network
        /// </summary>
        /// <returns>A <see cref="List{T}"/> containing, in order, the array with the inputs, the outputs and the ideal results</returns>
        public List<(double[] Inputs, double[] Outputs, double[] Ideals)> Test()
        {
            List<(double[] Inputs, double[] Outputs, double[] Ideals)> result = new List<(double[] Inputs, double[] Outputs, double[] Ideals)>();
            (double[] Inputs, double[] Outputs, double[] Ideals) item;

            foreach (IMLDataPair pair in trainingSet)
            {
                IMLData output = nn.Compute(pair.Input);

                item.Inputs = new double[pair.Input.Count];
                for (int i = 0; i < pair.Input.Count; i++)
                    item.Inputs[i] = pair.Input[i];

                item.Outputs = new double[output.Count];
                for (int i = 0; i < output.Count; i++)
                    item.Outputs[i] = output[i];

                item.Ideals = new double[pair.Ideal.Count];
                for (int i = 0; i < pair.Ideal.Count; i++)
                    item.Ideals[i] = pair.Ideal[i];

                result.Add(item);
            }

            return result;
        }
    }
}