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

        private CancellationTokenSource tokenSource;

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

        #endregion Constructors

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

            tokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Create the timer <see cref="Task"/>
        /// </summary>
        /// <returns>The timer (async) <see cref="Task"/></returns>
        private Task CreateTimerTask()
        {
            tokenSource.Cancel();
            tokenSource = new CancellationTokenSource();

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
                },
                tokenSource.Token
            );

            return t;
        }

        #endregion Private methods

        #region Public methods

        /// <summary>
        /// (Re)start the timer
        /// </summary>
        /// <remarks>
        /// Only to use in case of no <see cref="Condition"/> passed in the constructor!
        /// </remarks>
        public void Start()
            => CreateTimerTask().Start();

        #endregion Public methods
    }
}