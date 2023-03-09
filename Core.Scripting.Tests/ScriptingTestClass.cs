using Core.DataStructures;
using Diagnostic;
using NUnit.Framework;
using ScriptTest;
using System;
using System.IO;

namespace Core.Scripting.Tests
{
    public class ScriptingTestClass
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Initialize();

            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Meta", "Test", "scripts");

            ServiceBroker.Initialize();

            ScriptsService ss = new ScriptsService("ScriptsService", path);
            ServiceBroker.Provide(ss);

            ScriptManager.Initialize();
        }

        public void InitializeScript()
        {
            Test _ = new Test();
        }

        [Test]
        public void TestScript()
        {
            ScriptManager.Run();
        }
    }
}