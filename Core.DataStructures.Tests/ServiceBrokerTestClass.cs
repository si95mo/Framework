using FluentAssertions;
using Hardware;
using Hardware.Resources;
using NUnit.Framework;

namespace Core.DataStructures.Tests
{
    internal class ServiceBrokerTestClass
    {
        [OneTimeSetUp]
        public void Setup()
        {
            ServiceBroker.Initialize();
            ServiceBroker.Get<IProperty>().Count.Should().Be(0);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            ServiceBroker.Clear();
            ServiceBroker.Get<IProperty>().Count.Should().Be(0);
        }

        [Test]
        public void Add()
        {
            int index = 0;
            string serialResource = "SerialResource", tcpResource = "TcpResource";

            IResource resource = new SerialResource(serialResource);
            ServiceBroker.Add<IResource>(resource);

            ServiceBroker.Get<IResource>().Count.Should().Be(++index);

            resource = new TcpResource(tcpResource);
            ServiceBroker.Add<IResource>(resource);

            ServiceBroker.Get<IResource>().Count.Should().Be(++index);

            var resourceStored = ServiceBroker.Get<IResource>().Get(serialResource);
            resourceStored.Should().NotBeNull();
            resourceStored.Code.Should().Be(serialResource);

            resourceStored = ServiceBroker.Get<IResource>().Get(tcpResource);
            resourceStored.Should().NotBeNull();
            resourceStored.Code.Should().Be(tcpResource);

            index = 0;

            string analogIn = "AiChannel", analogOut = "AoChannel",
                digitalIn = "DiChannel", digitalOut = "DoChannel";

            IChannel channel = new AnalogInput(analogIn);
            ServiceBroker.Add<IChannel>(channel);

            ServiceBroker.Get<IChannel>().Count.Should().Be(++index);

            channel = new AnalogOutput(analogOut);
            ServiceBroker.Add<IChannel>(channel);

            ServiceBroker.Get<IChannel>().Count.Should().Be(++index);

            channel = new DigitalInput(digitalIn);
            ServiceBroker.Add<IChannel>(channel);

            ServiceBroker.Get<IChannel>().Count.Should().Be(++index);

            channel = new DigitalOutput(digitalOut);
            ServiceBroker.Add<IChannel>(channel);

            ServiceBroker.Get<IChannel>().Count.Should().Be(++index);

            var channelStored = ServiceBroker.Get<IChannel>().Get(analogIn);
            channelStored.Should().NotBeNull();
            channelStored.Code.Should().Be(analogIn);

            channelStored = ServiceBroker.Get<IChannel>().Get(analogOut);
            channelStored.Should().NotBeNull();
            channelStored.Code.Should().Be(analogOut);

            channelStored = ServiceBroker.Get<IChannel>().Get(digitalIn);
            channelStored.Should().NotBeNull();
            channelStored.Code.Should().Be(digitalIn);

            channelStored = ServiceBroker.Get<IChannel>().Get(digitalOut);
            channelStored.Should().NotBeNull();
            channelStored.Code.Should().Be(digitalOut);
        }

        [Test]
        public void ForeEach()
        {
            ServiceBroker.Clear();
            ServiceBroker.Get<IProperty>().Count.Should().Be(0);

            IChannel channel = new AnalogInput("AnalogIn");
            ServiceBroker.Add<IChannel>(channel);
            channel = new AnalogOutput("AnalogOut");
            ServiceBroker.Add<IChannel>(channel);

            foreach (IProperty item in ServiceBroker.Get<IProperty>())
                item.Should().NotBe(null);
        }
    }
}