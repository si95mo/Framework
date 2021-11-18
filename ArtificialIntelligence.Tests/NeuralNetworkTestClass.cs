using NUnit.Framework;

namespace ArtificialIntelligence.Tests
{
    [TestFixture]
    public class NeuralNetworkTestClass
    {
        private int[] layers = new int[3] { 5, 3, 2 };//initializing network to the right size

        //public List<Bot> Bots;
        public NeuralNetwork nn;

        [OneTimeSetUp]
        public void Setup()
        {
            nn = new NeuralNetwork(layers);
        }
    }
}
