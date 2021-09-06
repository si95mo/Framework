using Core.Threading;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Diagnostic.Tests
{
    public class DiagnosticTestClass
    {
        [OneTimeSetUp]
        public void Setup()
        {
            Logger.Initialize();
            LogReader.StartRead();

            Logger.Initialized.Should().BeTrue();
            LogReader.Reading.Should().BeTrue();
        }

        [Test]
        [TestCase(10000d)]
        public async Task LogReaderTest(double interval)
        {
            string lastLogText = "", lastLastLog = "";

            Stopwatch sw = Stopwatch.StartNew();
            do
            {
                Logger.Log(new Exception(sw.Elapsed.TotalMilliseconds.ToString("0.000")));

                await Tasks.NoOperation(2000);

                LogReader.LogText.Should().NotBe("").And.NotBe(lastLogText);
                LogReader.LastLog.Should().NotBe("").And.NotBe(lastLastLog);

                lastLogText = LogReader.LogText;
                lastLastLog = LogReader.LastLog;
            } while (sw.Elapsed.TotalMilliseconds <= interval);
        }
    }
}