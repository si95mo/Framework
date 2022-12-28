using Core.DataStructures;
using Diagnostic;
using NUnit.Framework;
using ScriptTest;

namespace Core.Scripting.Tests
{
    public class ScriptingTestClass
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Initialize();
            ServiceBroker.Initialize();
            InitializeScript();
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