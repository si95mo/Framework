using FluentAssertions;
using Hardware;
using NUnit.Framework;

namespace Core.DataStructures.Tests
{
    public class BagTestClass
    {
        private Bag<IChannel<double>> bag;

        [OneTimeSetUp]
        public void Init()
        {
            bag = new Bag<IChannel<double>>();

            bag.ItemAdded += Bag_ItemAdded;
            bag.ItemRemoved += Bag_ItemRemoved;

            var input = new AnalogInput("AiFirst");
            bag.Add(input);
            input = new AnalogInput("AiSecond");
            bag.Add(input);

            var output = new AnalogOutput("AoFirst");
            bag.Add(output);
            output = new AnalogOutput("AoSecond");
            bag.Add(output);

            bag.Count.Should().Be(4);
        }

        private void Bag_ItemAdded(object sender, BagChangedEventArgs<IProperty> e)
        {
            bag.Count.Should().BeGreaterThan(0);
        }

        private void Bag_ItemRemoved(object sender, BagChangedEventArgs<IProperty> e)
        {
            bool removed = bag.Remove(e.Item.Code);

            removed.Should().BeFalse();
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            bag.Clear();

            bag.Count.Should().Be(0);
        }

        [Test]
        [TestCase("AiFirst")]
        [TestCase("AiSecond")]
        [TestCase("AoFirst")]
        [TestCase("AoSecond")]
        public void GetItem(string code)
        {
            IChannel channel = bag.Get(code) as IChannel;

            channel.Code.Should().Be(code);
        }

        [Test]
        [TestCase("AiFirst")]
        [TestCase("AiSecond")]
        [TestCase("AoFirst")]
        [TestCase("AoSecond")]
        public void RemoveItem(string code)
        {
            IProperty channel;

            if (bag.Count > 0)
                channel = bag.Get(code);
            else
            {
                channel = new AnalogInput(code);
                bag.Add(channel);
            }

            bool removed = bag.Remove(channel);

            removed.Should().BeTrue();
        }
    }
}