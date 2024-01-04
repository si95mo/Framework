using FluentAssertions;
using Nancy.Testing;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Rest.Tests
{
    [TestFixture]
    public class RestTestClass
    {
        private RestServer server;

        [OneTimeSetUp]
        public async Task Setup()
        {
            ConfigurableBootstrapper bootstrapper = new ConfigurableBootstrapper((x) => x.AddInfo());
            try
            {
                server = new RestServer(nameof(RestServer), 12345, bootstrapper);

                await server.Start();
                server.Status.Value.Should().Be(Hardware.ResourceStatus.Executing);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            server.Stop();
            server.Status.Value.Should().Be(Hardware.ResourceStatus.Stopped);
        }

        [Test]
        [TestCase(100d)]
        public async Task Wait(double secondsToWait)
        {
            await Task.Delay(TimeSpan.FromSeconds(secondsToWait));
        }
    }
}
