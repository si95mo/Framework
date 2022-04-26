using Core.Parameters;
using System;
using System.Diagnostics;
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
        /// The actual elapsed time
        /// </summary>
        public TimeSpanParameter ElapsedTime { get; private set; }

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
            InitializeParameters(time);
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
            InitializeParameters(TimeSpan.Zero);

            tokenSource = new CancellationTokenSource();
            withEndCondition = true;

            startCondition.ValueChanged += StartCondition_ValueChanged;
            endCondition.ValueChanged += EndCondition_ValueChanged;
        }

        /// <summary>
        /// Initialize specific class parameters
        /// </summary>
        /// <param name="time">The <see cref="Condition"/> time</param>
        private void InitializeParameters(TimeSpan time)
        {
            Time = new TimeSpanParameter($"{Code}.{nameof(Time)}", time);
            Started = new BoolParameter($"{Code}.{nameof(Started)}", false);
            ElapsedTime = new TimeSpanParameter($"{Code}.{nameof(ElapsedTime)}", 0d);
        }

        private void StartCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if ((bool)e.NewValue)
            {
                if (!withEndCondition)
                    CreateTimerTask().Start();
                else
                    CreateTimerTask(tokenSource.Token).Start();
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
                    Stopwatch timer;

                    if (!Started.Value)
                    {
                        Started.Value = true;

                        timer = Stopwatch.StartNew();
                        Value = false;

                        await Task.Delay(Time.Value);

                        Value = true;
                        timer.Stop();

                        ElapsedTime.Value = TimeSpan.FromMilliseconds(timer.Elapsed.TotalMilliseconds);

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
                    Stopwatch timer;

                    if (!Started.Value)
                    {
                        Started.Value = true;

                        timer = Stopwatch.StartNew();
                        Value = false;

                        await Task.Delay(-1, token)
                            .ContinueWith((x) =>
                                {
                                    Value = true;
                                    timer.Stop();

                                    ElapsedTime.Value = TimeSpan.FromMilliseconds(timer.Elapsed.TotalMilliseconds);

                                    Started.Value = false;
                                }
                            );
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