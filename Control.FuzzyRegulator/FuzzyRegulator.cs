using Core;
using Core.Parameters;
using Core.Threading;
using Hardware;
using Mathematics.FuzzyLogic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Control.FuzzyRegulator
{
    /// <summary>
    /// Implement a <see cref="Regulator"/> that uses fuzzy logic to calculate the control output.
    /// See also <see cref="FuzzySystem"/>
    /// </summary>
    public class FuzzyRegulator : Regulator
    {
        private FuzzySystem fuzzySystem;
        private Dictionary<FuzzyVariable, IProperty<double>> inputVariables;
        private FuzzyVariable outputVariable;

        private Task controlTask;
        private bool doRegulate;
        private TimeSpan timeSinceLastUpdate;

        /// <summary>
        /// The cycle time
        /// </summary>
        public TimeSpanParameter CycleTime { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="FuzzyRegulator"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="feedbackChannel">The feedback <see cref="Channel{T}"/></param>
        /// <param name="setpoint">The setpoint</param>
        /// <param name="fuzzySystem">The <see cref="FuzzySystem"/></param>
        /// <param name="inputVariables">
        /// The <see cref="Dictionary{TKey, TValue}"/> with all the <paramref name="fuzzySystem"/>
        /// input <see cref="FuzzyVariable"/> with the relative <see cref="IProperty"/>
        /// </param>
        /// <param name="outputVariable">The <paramref name="fuzzySystem"/> output <see cref="FuzzyVariable"/></param>
        /// <param name="cycleTime">The cycle time (as <see cref="TimeSpan"/>)</param>
        public FuzzyRegulator(string code, Channel<double> feedbackChannel, double setpoint, FuzzySystem fuzzySystem,
            Dictionary<FuzzyVariable, IProperty<double>> inputVariables, FuzzyVariable outputVariable, TimeSpan cycleTime)
            : base(code, feedbackChannel, setpoint)
        {
            this.fuzzySystem = fuzzySystem;
            this.inputVariables = inputVariables;
            this.outputVariable = outputVariable;

            CycleTime = new TimeSpanParameter($"{Code}.{nameof(CycleTime)}", cycleTime);

            controlTask = null;
            doRegulate = false;
            timeSinceLastUpdate = TimeSpan.Zero;
        }

        /// <summary>
        /// Create a new instance of <see cref="FuzzyRegulator"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="feedbackChannel">The feedback <see cref="Channel{T}"/></param>
        /// <param name="setpoint">The setpoint. The actual setpoint is given by the <paramref name="fuzzySystem"/> rules' set</param>
        /// <param name="fuzzySystem">The <see cref="FuzzySystem"/></param>
        /// <param name="cycleTime">The cycle time (in milliseconds)</param>
        public FuzzyRegulator(string code, Channel<double> feedbackChannel, double setpoint, FuzzySystem fuzzySystem,
            Dictionary<FuzzyVariable, IProperty<double>> variables, FuzzyVariable outputVariable, int cycleTime)
            : this(code, feedbackChannel, setpoint, fuzzySystem, variables, outputVariable, TimeSpan.FromMilliseconds(cycleTime))
        { }

        /// <summary>
        /// Create a new controlling <see cref="Task"/>
        /// </summary>
        /// <returns>The controlling <see cref="Task"/></returns>
        private Task CreateControlTask()
            => new Task(async () =>
                {
                    doRegulate = true;

                    Stopwatch sw;
                    int timeToWait;

                    while (doRegulate)
                    {
                        sw = Stopwatch.StartNew();

                        Iterate();

                        timeToWait = (int)(CycleTime.Value.TotalMilliseconds - sw.Elapsed.TotalMilliseconds);
                        if (timeToWait > 0)
                            await Tasks.NoOperation(timeToWait, 1);

                        timeSinceLastUpdate = new TimeSpan(sw.Elapsed.Ticks).Subtract(timeSinceLastUpdate);
                    }
                }
            );

        /// <summary>
        /// Perform an iteration of the control algorithm
        /// </summary>
        private void Iterate()
        {
            fuzzySystem.Clear();

            foreach (KeyValuePair<FuzzyVariable, IProperty<double>> variable in inputVariables)
                fuzzySystem.AddInput(variable.Key, variable.Value.Value);

            fuzzySystem.AddOutput(outputVariable);

            List<string> results = fuzzySystem.Calculate();
            uk.Value = fuzzySystem.GetResultAsDouble(results, 0);
        }

        public override void Start()
        {
            if (controlTask == null)
            {
                controlTask = CreateControlTask();
                controlTask.Start();
            }
            else
            {
                controlTask.Wait((int)CycleTime.Value.TotalMilliseconds);
                controlTask.Dispose();

                controlTask = CreateControlTask();
                controlTask.Start();
            }
        }

        /// <summary>
        /// Stop the <see cref="FuzzyRegulator"/> controller
        /// </summary>
        public void Stop()
        {
            doRegulate = false;
            timeSinceLastUpdate = new TimeSpan(0);
        }
    }
}