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
                @"test_results//test_logs//"
            );

            Logger.Initialize(path);
            LogReader.StartRead();
            Timer.Initialize();

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

        [Test]
        [TestCase(10)]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        public async Task TimerTest(int interval)
        {
            Timer.Start(); // await for interval ms
            Timer.Start(); // await for (2 * interval + interval) ms

            await Task.Delay(interval);
            double firstElapsed = Timer.GetElapsedTime(); // Stop the first enqueued timer and retrieve its timestamp

            await Task.Delay(2 * interval);
            double secondElapsed = Timer.GetElapsedTime(); // Stop the second enqueued timer and retrieve its timestamp

            firstElapsed.Should().BeApproximately(interval, 16);
            secondElapsed.Should().BeApproximately(3 * interval, 2 * 16);
        }

        [Test]
        public async Task LogAsyncTest()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                @"test_results//test_logs_async//"
            );
            Logger.Initialize(path);

            int n = 0;
            byte[] buffer;
            for (int i = 0; i < 100; i++)
            {
                await Logger.InfoAsync((i + 1).ToString());

                buffer = File.ReadAllBytes(Logger.Path);
                buffer.Length.Should().NotBe(n);

                n = buffer.Length;
            }

            await Logger.LogAsync(new Exception("Test Exception message"));
            buffer = File.ReadAllBytes(Logger.Path);
            buffer.Length.Should().NotBe(n);
        }

        [Test]
        public async Task ConcurrencyTest()
        {
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                @"test_results//test_logs_async//"
            );
            Logger.Initialize(path);
            Logger.SetMinimumSeverityLevel(Severity.Trace);

            int initialSize = File.ReadAllBytes(Logger.Path).Length;

            Action firstAction = new Action(async () =>
                {
                    for (int i = 1; i <= 10; i++)
                        await Logger.DebugAsync(i.ToString());
                }
            );

            Action secondAction = new Action(async () =>
                {
                    for (int i = 10; i >= 1; i--)
                        await Logger.TraceAsync(i.ToString());
                }
            );

            Parallel.Invoke(
                () => firstAction(),
                () => secondAction()
            );

            await Task.Delay(1000); // await some time

            byte[] buffer = File.ReadAllBytes(Logger.Path);

            // Each of the 20 lines is 74 bytes, if the number is single digit.
            // There are 2 double digit numbers saved, plus 2 '-' character saved, thus 4 bytes in total
            buffer.Length.Should().Be(20 * 74 + initialSize + 4); 
        }
    }
}