using Core.Parameters;
using FluentAssertions;
using Hardware;
using NUnit.Framework;

namespace Core.DataStructures.Tests
{
    public class BagTestClass
    {
        private Bag<IChannel> bag;

        [OneTimeSetUp]
        public void Init()
        {
            bag = new Bag<IChannel>();

            bag.Added += Bag_ItemAdded;
            bag.Removed += Bag_ItemRemoved;

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

        private void Bag_ItemAdded(object sender, BagChangedEventArgs<IChannel> e)
        {
            bag.Count.Should().BeGreaterThan(0);
        }

        private void Bag_ItemRemoved(object sender, BagChangedEventArgs<IChannel> e)
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
            IChannel channel = bag.Get(code);
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
                bag.Add(channel as AnalogInput);
            }

            bool removed = bag.Remove(channel.Code);
            removed.Should().BeTrue();
        }

        [Test]
        public void ForeEach()
        {
            foreach (IProperty property in bag)
                property.Code.Should().NotBeNullOrEmpty();
        }
    }
}