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
    public class TimeElapsedCondition : FlyweightCondition
    {
        private CancellationTokenSource tokenSource;
        private bool withEndCondition;

        /// <summary>
        /// The time after which the <see cref="TimeElapsedCondition"/> <see cref="Condition"/>
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

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsedCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/> (as a <see cref="TimeSpan"/></param>
        public TimeElapsedCondition(string code, TimeSpan time) : base(code)
        {
            InitializeParameters(time);
        }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsedCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/> (in milliseconds)</param>
        public TimeElapsedCondition(string code, double time) : this(code, TimeSpan.FromMilliseconds(time))
        { }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsedCondition"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="time">The time after which the new instance will be <see langword="true"/></param>
        /// <param name="startCondition">The <see cref="ICondition"/> that will start the timer when will become <see langword="true"/></param>
        public TimeElapsedCondition(string code, TimeSpan time, ICondition startCondition) : this(code, time)
        {
            withEndCondition = false;

            startCondition.ValueChanged += StartCondition_ValueChanged;
        }

        /// <summary>
        /// Create a new instance of <see cref="TimeElapsedCondition"/>
        /// </summary>
        /// <remarks>
        /// The type of instance of <see cref="TimeElapsedCondition"/> created with this type of constructor is totally automated and does
        /// not became <see langword="true"/> after a certain amount of time passed, but only when 2 other <see cref="ICondition"/> switch to <see langword="true"/>
        /// </remarks>
        /// <param name="code">The code</param>
        /// <param name="startCondition">The <see cref="ICondition"/> that will start the timer when will become <see langword="true"/></param>
        /// <param name="endCondition">The <see cref="ICondition"/> that will stop the timer when will become <see langword="true"/></param>
        public TimeElapsedCondition(string code, ICondition startCondition, ICondition endCondition) : base(code)
        {
            InitializeParameters(TimeSpan.Zero);

            tokenSource = new CancellationTokenSource();
            withEndCondition = true;

            startCondition.ValueChanged += StartCondition_ValueChanged;
            endCondition.ValueChanged += EndCondition_ValueChanged;
        }

        #endregion Constructors

        #region Event handlers

        private void StartCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValueAsBool)
            {
                if (!withEndCondition)
                    CreateTimerTask().Start();
                else
                    CreateTimerTask(tokenSource.Token).Start();
            }
        }

        private void EndCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValueAsBool)
                tokenSource.Cancel();
        }

        #endregion Event handlers

        #region Private methods

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

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// Start the timer
        /// </summary>
        /// <remarks>
        /// Only to use in case of no <see cref="Condition"/> passed in the constructor!
        /// </remarks>
        public void Start()
            => CreateTimerTask().Start();

        #endregion Public methods
    }
}