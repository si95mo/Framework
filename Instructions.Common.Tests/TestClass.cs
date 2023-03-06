using Core.Parameters;
using Core.Scheduling;
using Extensions;
using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Instructions.Common.Tests
{
    [TestFixture]
    public class TestClass
    {
        private const double TimeoutInMilliseconds = 10000;

        private InstructionScheduler scheduler;
        private WaitForCondition waitForCondition;

        private NumericParameter firstValue, secondValue;

        [OneTimeSetUp]
        public void Setup()
        {
            scheduler = new InstructionScheduler();
            firstValue = new NumericParameter("FirstValue");
            secondValue = new NumericParameter("SecondValue");

            TimeSpan timeout = TimeSpan.FromMilliseconds(TimeoutInMilliseconds); // 10 seconds
            TimeSpan conditionTime = TimeSpan.FromMilliseconds(1000); // 1 second
            waitForCondition = new WaitForCondition(timeout, conditionTime, firstValue, secondValue, Operand.GreatherOrEqual);
        }

        /// <summary>
        /// Add the <see cref="IInstruction"/> to the <see cref="InstructionScheduler"/>
        /// </summary>
        private void AddInstructions()
        {
            scheduler.Add(waitForCondition);
        }

        /// <summary>
        /// Set the values to the <see cref="IInstruction"/> related variables
        /// </summary>
        /// <param name="firstValue">The first value</param>
        /// <param name="secondValue">The second value</param>
        private void SetValues(double firstValue, double secondValue)
        {
            this.firstValue.Value = firstValue;
            this.secondValue.Value = secondValue;
        }

        [Test]
        [TestCase(10d, 20d)]
        [TestCase(4d, 4d)]
        public async Task ExecuteWithSuccess(double x, double y)
        {
            AddInstructions();
            SetValues(x, y); // Greater or equal

            Stopwatch sw = Stopwatch.StartNew();
            List<IInstruction> executedInstructions = await scheduler.Execute();
            sw.Stop();

            sw.Elapsed.TotalMilliseconds.Should().BeLessThan(TimeoutInMilliseconds);
            foreach (IInstruction instruction in executedInstructions)
            {
                instruction.Succeeded.Value.Should().BeTrue();
                instruction.Failed.Value.Should().BeFalse();
            }
        }

        [Test]
        [TestCase(1.5, 1.4)]
        [TestCase(0d, -1d)]
        public async Task ExecuteWithoutSuccess(double x, double y)
        {
            AddInstructions();
            SetValues(x, y); // Greather or equal

            Stopwatch sw = Stopwatch.StartNew();
            List<IInstruction> executedInstructions = await scheduler.Execute();
            sw.Stop();

            sw.Elapsed.TotalMilliseconds.Should().BeGreaterThan(TimeoutInMilliseconds);
            foreach (IInstruction instruction in executedInstructions)
            {
                instruction.Succeeded.Value.Should().BeFalse();
                instruction.Failed.Value.Should().BeTrue();
            }
        }

        [Test]
        [TestCase(1d, 2d)] // Success
        [TestCase(2d, 1d)] // Fail
        public async Task TestSequence(double x, double y)
        {
            Sequence sequence = new Sequence("Sequence", 0);
            WaitForCondition waitFor = new WaitForCondition(
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(0.5),
                x.WrapToParameter(),
                y.WrapToParameter(),
                Operand.Greather
            );

            sequence.Add(waitFor);
            sequence.Add(waitFor.DeepCopy());

            scheduler.RemoveAll();
            scheduler.Add(sequence);

            List<IInstruction> executedInstructions = await scheduler.Execute();

            foreach (IInstruction instruction in executedInstructions)
            {
                if (x == 1d)
                {
                    instruction.Succeeded.Value.Should().BeTrue();
                    instruction.Failed.Value.Should().BeFalse();
                }
                else
                {
                    instruction.Succeeded.Value.Should().BeFalse();
                    instruction.Failed.Value.Should().BeTrue();
                }
            }
        }

        [Test]
        [TestCase(1000d)]
        public async Task TestParallel(double time)
        {
            Parallel parallel = new Parallel("Parallel", 0);
            Pause pause = new Pause(time);

            parallel.Add(pause);
            parallel.Add(pause.DeepCopy());

            scheduler.RemoveAll();
            scheduler.Add(parallel);

            Stopwatch sw = Stopwatch.StartNew();
            await scheduler.Execute();
            sw.Stop();

            sw.Elapsed.TotalMilliseconds.Should().BeApproximately(time, 100d);
        }
    }
}