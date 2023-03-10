using Core.DataStructures;
using Diagnostic;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Tasks.Tests
{
    public class FunctionTask : AwaitableWithAlarm
    {
        public FunctionTask(string code, Scheduler scheduler = null) : base(code, scheduler)
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
            ServiceBroker.Initialize();
        }

        [Test]
        public async Task Test()
        {
            Scheduler scheduler = new Scheduler(maxDegreesOfParallelism: 10);
            List<IAwaitable> tasks = new List<IAwaitable>();
            for(int i = 0; i < 20; i++)
                tasks.Add(new FunctionTask(i.ToString(), scheduler));

            tasks.ForEach((x) => x.Start());

            IAwaitable task = new FunctionTask("Code", scheduler);

            Stopwatch timer = Stopwatch.StartNew();
            await task;
            timer.Stop();

            Console.WriteLine($"Elapsed milliseconds: {timer.Elapsed.TotalMilliseconds}");

            timer.Elapsed.TotalMilliseconds.Should().BeApproximately(2000d, 200d); // 2000ms +/- 200ms
            task.Status.Value.Should().Be(TaskStatus.Faulted);
        }
    }
}