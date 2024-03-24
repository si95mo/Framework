using FluentAssertions;
using Hardware.Mqtt;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Hardware.Resources.Tests
{
    /// <summary>
    /// 1) Open https://console.hivemq.cloud/clusters/free/067b28a96a9147aa9c72596a70697389 <br/>
    /// 2) Connect the web client https://console.hivemq.cloud/clusters/free/067b28a96a9147aa9c72596a70697389/web-client <br/>
    /// 3) Test the MQTT communication: <br/>
    /// --> 3.1) Send the message {  Id: "ABC123", Value: 1  } from the web client in the "Test" topic<br/>
    /// --> 3.2) Check in the web client if the sent messages arrive in the "Test" topic <br/>
    /// </summary>
    internal class MqttTransferModel
    {
        public string Id { get; set; } = string.Empty;
        public int Value { get; set; } = int.MinValue;

        public MqttTransferModel()
        { }

        public MqttTransferModel(string id, int value)
        {
            Id = id;
            Value = value;
        }

        public override string ToString()
        {
            string description = JsonConvert.SerializeObject(this);
            return description;
        }
    }

    [TestFixture]
    public class MqttTestClass
    {
        private MqttClientResource resource;
        private MqttInputChannel<MqttTransferModel> inputChannel;
        private MqttOutputChannel<MqttTransferModel> outputChannel;
        private int counter;

        [OneTimeSetUp]
        public async Task OneTimeSetUp()
        {
            resource = new MqttClientResource("MqttResource", "067b28a96a9147aa9c72596a70697389.s1.eu.hivemq.cloud", port: 8883, username: "BloomDee", password: "E6QsF#_KxZYubyp", withTls: true);
            await resource.Start();

            inputChannel = new MqttInputChannel<MqttTransferModel>("MqttInputChannel", resource, "Test");
            outputChannel = new MqttOutputChannel<MqttTransferModel>("MqttOutputChannel", resource, "Test", QualityOfService.AtLeastOnce);
        }

        #region Subscription test

        [Test]
        [TestCase(100000)]
        public async Task SubscribeTestAsync(int timeoutInMilliseconds)
        {
            counter = 0;

            inputChannel.ValueChanged += Channel_ValueChanged;
            await Task.Delay(timeoutInMilliseconds);
            inputChannel.ValueChanged -= Channel_ValueChanged;

            counter.Should().BeGreaterThan(0);
        }

        private void Channel_ValueChanged(object sender, Core.ValueChangedEventArgs e)
        {
            counter++;

            Console.WriteLine(
                $"{DateTime.Now:HH:mm:ss.fff} >> Value changed {counter} times.{Environment.NewLine}" +
                $"{new string(Enumerable.Repeat(' ', "HH:mm:ss.fff".Length).ToArray())} >> Actual value is {inputChannel.Value}"
            );
        }

        #endregion Subscription test

        #region Publish test

        [Test]
        [TestCase("A", 1)]
        [TestCase("B", 2)]
        [TestCase("C", 3)]
        [TestCase("D", 4)]
        public async Task PublishTestAsync(string id, int value)
        {
            MqttTransferModel model = new MqttTransferModel(id, value);
            outputChannel.Value = model;

            await Task.Delay(1000);
        }

        #endregion Publish test
    }
}
