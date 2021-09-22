using Core.Threading;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Diagnostic.Tests
{
    public class DiagnosticTestClass
    {
        [OneTimeSetUp]
        public void Setup()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                @"test_logs\"
            );

            Logger.Initialize(path);
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
                try
                {
                    File.ReadAllText("dummy path");
                }
                catch (Exception ex)
                {
                    Logger.Log($"Exception throw at {sw.Elapsed.TotalMilliseconds:HH:mm:ss:fff}", Severity.Info);
                    Logger.Log(ex);
                }

                await Tasks.NoOperation(2000);

                LogReader.LogText.Should().NotBe("").And.NotBe(lastLogText);
                LogReader.LastLog.Should().NotBe("").And.NotBe(lastLastLog);

                lastLogText = LogReader.LogText;
                lastLastLog = LogReader.LastLog;
            } while (sw.Elapsed.TotalMilliseconds <= interval);
        }
    }
}