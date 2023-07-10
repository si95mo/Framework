using NUnit.Framework;
using System;
using System.IO;

namespace Configurations.Tests
{
    [TestFixture]
    public class TestClass
    {
        private Configuration.Configuration config;

        [OneTimeSetUp]
        public void Setup()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "test_results", "config.json");
            config = new Configuration.Configuration(path);
        }

        [Test]
        public void Test()
        {
            ;
        }
    }
}