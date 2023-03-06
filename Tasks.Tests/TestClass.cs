using Core.Conditions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Extensions;
using Diagnostic;

namespace Tasks.Tests
{
    public class FunctionTask : AwaitableWithAlarm
    {
        public FunctionTask(string code) : base(code)
        { }

        public override IEnumerable<string> Execution()
        {
            yield return WaitFor(TimeSpan.FromMilliseconds(1000d));
            yield return WaitFor(Task.Delay(1000));

            FireAlarm("Custom message");

            yield return "Done";
        }

        protected override void OnFail()
        {
            WaitState.Value = $"{nameof(OnFail)} method called";
            base.OnFail();
        }
    }

    [TestFixture]
    public class TestClass
    {
        [OneTimeSetUp] 
        public void SetUp()
        {
            Logger.Initialize();
        }

        [Test] 
        public async Task Test()
        {
            IAwaitable task = new FunctionTask("Code");

            Stopwatch timer = Stopwatch.StartNew();
            await task;
            timer.Stop();

            Console.WriteLine($"Elapsed milliseconds: {timer.Elapsed.TotalMilliseconds}");

            timer.Elapsed.TotalMilliseconds.Should().BeApproximately(2000d, 200d); // 2000ms +/- 200ms
            task.Status.Value.Should().Be(TaskStatus.Faulted);
        }
    }
}
