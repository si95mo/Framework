using Core.DataStructures;
using Core.DataStructures.Extensions;
using Diagnostic;
using FluentAssertions;
using NUnit.Framework;
using ScriptTest;
using System;
using System.IO;
using System.Linq;

namespace Core.Scripting.Tests
{
    public class ScriptingTestClass
    {
        private ScriptsService ss;

        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Initialize();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Meta", "Test", "scripts");

            ServiceBroker.Initialize();

            ss = new ScriptsService("ScriptsService", path);
            ServiceBroker.Provide(ss);

            ScriptManager.Initialize();
        }

        [Test]
        public void TestScript()
        {
            ScriptManager.Run();
            (ss.ToList().FirstOrDefault() as IScript).Ran.Should().BeTrue();
            (ss.ToList().FirstOrDefault() as IScript).Cleared.Should().BeFalse();

            ScriptManager.Clear();
            (ss.ToList().FirstOrDefault() as IScript).Ran.Should().BeFalse();
            (ss.ToList().FirstOrDefault() as IScript).Cleared.Should().BeTrue();
        }
    }
}