using Core.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Conditions
{
    /// <summary>
    /// Implement a condition that will be <see langword="true"/> after 
    /// a time interval elapsed
    /// </summary>
    public class TimeElapsed : FlyweightCondition
    {
        private CancellationTokenSource tokenSource;
        private bool withEndCondition;

        /// <summary>
        /// The time after which the <see cref="TimeElapsed"/> <see cref="Condition"/> 
        /// will be  <see langword="true"/>
        /// </summary>
        public TimeSpanParameter Time { get; set; }
        
        /// <summary>
        /// The started state
        /// </summary>
        public BoolParameter Started { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsed"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/> (as a <see cref="TimeSpan"/></param>
        public TimeElapsed(string code, TimeSpan time) : base(code)
        {
            Time = new TimeSpanParameter($"{Code}.{nameof(Time)}", time);
            Started = new BoolParameter($"{Code}.{nameof(Started)}", false);
        }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsed"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/> (in milliseconds)</param>
        public TimeElapsed(string code, double time) : this(code, TimeSpan.FromMilliseconds(time))
        { }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsed"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/></param>
        /// <param name="startCondition">The <see cref="ICondition"/> that will start the timer when will become <see langword="true"/></param>
        public TimeElapsed(string code, TimeSpan time, ICondition startCondition) : this(code, time)
        {
            withEndCondition = false;

            startCondition.ValueChanged += StartCondition_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsed"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/></param>
        /// <param name="startCondition">The <see cref="ICondition"/> that will start the timer when will become <see langword="true"/></param>
        /// <param name="endCondition">The <see cref="ICondition"/> that will stop the timer when will become <see langword="true"/></param>
        public TimeElapsed(string code, ICondition startCondition, ICondition endCondition) : base(code)
        {
            tokenSource = new CancellationTokenSource();
            withEndCondition = true;

            startCondition.ValueChanged += StartCondition_ValueChanged;
            endCondition.ValueChanged += EndCondition_ValueChanged;
        }

        private void StartCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (!withEndCondition)
                    CreateTimerTask().Start();
                else
                    CreateTimerTask(tokenSource.Token);
            }
        }

        private void EndCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                tokenSource.Cancel();
        }

        /// <summary>
        /// Create the timer <see cref="Task"/>
        /// </summary>
        /// <returns>The timer (async) <see cref="Task"/></returns>
        private Task CreateTimerTask()
        {
            Task t = new Task(async () =>
                {
                    if (!Started.Value)
                    {
                        Started.Value = true;

                        Value = false;
                        await Task.Delay(Time.Value);
                        Value = true;

                        Started.Value = false;
                    }
                }
            );

            return t;
        }


        /// <summary>
        /// Create the timer <see cref="Task"/> with a <see cref="CancellationToken"/>
        /// </summary>
        /// <param name="token">The <see cref="CancellationToken"/></param>
        /// <returns>The timer (async) <see cref="Task"/></returns>
        private Task CreateTimerTask(CancellationToken token)
        {
            Task t = new Task(async () =>
                {
                    if (!Started.Value)
                    {
                        Started.Value = true;

                        Value = false;
                        await Task.Delay(Time.Value, token).ContinueWith((x) => { }); // To prevent Exception throw
                        Value = true;

                        Started.Value = false;
                    }
                }
            );

            return t;
        }

        /// <summary>
        /// Start the timer
        /// </summary>
        /// <remarks>
        /// Only to use in case of no <see cref="Condition"/> passed in the constructor!
        /// </remarks>
        public void Start()
            => CreateTimerTask().Start();
    }
}
