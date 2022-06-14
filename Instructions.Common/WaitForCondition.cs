using Core.Parameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Instructions.Common
{
    /// <summary>
    /// Define the type of operand to use in the
    /// <see cref="WaitForCondition"/> test
    /// </summary>
    public enum Operand
    {
        /// <summary>
        /// '==' operand
        /// </summary>
        Equal = 0,

        /// <summary>
        /// '!=' operand
        /// </summary>
        NotEqual = 1,

        /// <summary>
        /// '>' operand
        /// </summary>
        Greather = 2,

        /// <summary>
        /// '<' operand
        /// </summary>
        Less = 3,

        /// <summary>
        /// '>=' operand
        /// </summary>
        GreatherOrEqual = 4,

        /// <summary>
        /// '<=' operand
        /// </summary>
        LessOrEqual = 5
    }

    /// <summary>
    /// Implement an <see cref="Instruction"/> that waits for a condition to be met,
    /// or for a timeout
    /// </summary>
    public class WaitForCondition : Instruction
    {
        private TimeSpanParameter timeout;
        private NumericParameter firstValue, secondValue;
        private EnumParameter<Operand> operand;
        private TimeSpanParameter conditionTime;

        private int pollingInterval;
        private Task waitTask;

        /// <summary>
        /// Create a new instance of <see cref="WaitForCondition"/>
        /// </summary>
        /// <remarks>
        /// The test is applied as follows, assuming <paramref name="operand"/> is <see cref="Operand.Greather"/>: <br/>
        /// <paramref name="secondValue"/> > <paramref name="firstValue"/>
        /// </remarks>
        /// <param name="code">The code</param>
        /// <param name="timeout">The condition timeout</param>
        /// <param name="conditionTime">The time in which the condition has to remain <see langword="true"/></param>
        /// <param name="firstValue">The first value to test</param>
        /// <param name="secondValue">The second value to test</param>
        /// <param name="operand">The <see cref="Operand"/> to apply</param>
        /// <param name="pollingInterval">The polling interval (in milliseconds)</param>
        public WaitForCondition(TimeSpan timeout, TimeSpan conditionTime, NumericParameter firstValue, NumericParameter secondValue,
            Operand operand, int pollingInterval = 50) : base("WaitForCondition")
        {
            this.timeout = new TimeSpanParameter($"{Code}.Timeout", timeout);
            this.conditionTime = new TimeSpanParameter($"{Code}.ConditionTime", conditionTime);
            this.firstValue = new NumericParameter($"{Code}.FirstValue", firstValue.Value);
            this.secondValue = new NumericParameter($"{Code}.SecondValue", secondValue.Value);
            this.operand = new EnumParameter<Operand>($"{Code}.Operand", operand);

            this.pollingInterval = pollingInterval;

            firstValue.ConnectTo(this.firstValue);
            secondValue.ConnectTo(this.secondValue);

            InputParameters.Add(this.timeout);
            InputParameters.Add(this.conditionTime);
            InputParameters.Add(this.firstValue);
            InputParameters.Add(this.secondValue);
            InputParameters.Add(this.operand);
        }

        public override void OnStart()
        {
            waitTask = Task.Run(async () =>
                {
                    while (!TestCondition())
                        await Task.Delay(pollingInterval);

                    Stopwatch sw = Stopwatch.StartNew();
                    while (sw.Elapsed.TotalMilliseconds < conditionTime.Value.TotalMilliseconds != !TestCondition())
                        await Task.Delay(pollingInterval);

                    sw.Stop();
                }
            );

            base.OnStart();
        }

        public override async Task ExecuteInstruction()
        {
            Task completedTask = await Task.WhenAny(waitTask, Task.Delay(timeout.Value));
            if (completedTask != waitTask)
                Fail();
            else
                Succeeded.Value = true;
        }

        /// <summary>
        /// Test to condition to apply based on the <see cref="Operand"/> to use
        /// </summary>
        /// <returns><see langword="true"/> if the condition is met, <see langword="false"/> otherwise</returns>
        private bool TestCondition()
        {
            bool conditionMet = false;

            switch (operand.Value)
            {
                case Operand.Equal:
                    conditionMet = Math.Abs(firstValue.Value - secondValue.Value) == 0;
                    break;

                case Operand.NotEqual:
                    conditionMet = Math.Abs(firstValue.Value - secondValue.Value) != 0;
                    break;

                case Operand.Greather:
                    conditionMet = secondValue.Value > firstValue.Value;
                    break;

                case Operand.Less:
                    conditionMet = secondValue.Value < firstValue.Value;
                    break;

                case Operand.GreatherOrEqual:
                    conditionMet = secondValue.Value >= firstValue.Value;
                    break;

                case Operand.LessOrEqual:
                    conditionMet = secondValue.Value <= firstValue.Value;
                    break;
            }

            return conditionMet;
        }
    }
}