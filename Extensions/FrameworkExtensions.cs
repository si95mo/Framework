using Core;
using Core.Conditions;
using Core.Parameters;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Provides framework-related extension methods
    /// </summary>
    public static class FrameworkExtension
    {
        /// <summary>
        /// Wrap a <see cref="double"/> to a <see cref="NumericParameter"/>
        /// </summary>
        /// <param name="source">The source</param>
        /// <returns>The <see cref="NumericParameter"/></returns>
        public static NumericParameter WrapToParameter(this double source)
            => new NumericParameter($"{nameof(source)}.AsParameter", source);

        /// <summary>
        /// Wrap a <see cref="bool"/> to a <see cref="BoolParameter"/>
        /// </summary>
        /// <param name="source">The source</param>
        /// <returns>The <see cref="BoolParameter"/></returns>
        public static BoolParameter WrapToParameter(this bool source)
            => new BoolParameter($"{nameof(source)}.AsParameter", source);

        /// <summary>
        /// Wrap a <see cref="string"/> to a <see cref="StringParameter"/>
        /// </summary>
        /// <param name="source">The source</param>
        /// <returns>The <see cref="StringParameter"/></returns>
        public static StringParameter WrapToParameter(this string source)
            => new StringParameter($"{nameof(source)}.AsParameter", source);

        /// <summary>
        /// Wrap a <see cref="TimeSpan"/> to a <see cref="TimeSpanParameter"/>
        /// </summary>
        /// <param name="source">The source</param>
        /// <returns>The <see cref="TimeSpanParameter"/></returns>
        public static TimeSpanParameter WrapToParameter(this TimeSpan source)
            => new TimeSpanParameter($"{nameof(source)}.AsParameter", source);

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/> without timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this object _, ICondition condition)
        {
            if (!condition.Value)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                EventHandler<ValueChangedEventArgs> eventHandler = (__, ___) =>
                {
                    if (condition.Value)
                        tokenSource.Cancel();
                };

                condition.ValueChanged += eventHandler;
                await Task.Delay(-1, tokenSource.Token);
                condition.ValueChanged -= eventHandler;
            }
        }

        /// <summary>
        /// Wait for an <see cref="ICondition"/> to be <see langword="true"/> with a timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="condition">The <see cref="ICondition"/> to wait</param>
        /// <param name="timeout">The timeout (in milliseconds)</param>
        /// <returns>
        /// The (async) <see cref="Task{T}"/> (in which the result will be <see langword="true"/> if
        /// the <paramref name="condition"/> became <see langword="true"/> before <paramref name="timeout"/> occurred,
        /// <see langword="false"/> otherwise)
        /// </returns>
        public static async Task<bool> WaitFor(this object _, ICondition condition, int timeout)
        {
            bool result = true;

            if(!condition.Value)
            {
                CancellationTokenSource tokenSource = new CancellationTokenSource();
                EventHandler<ValueChangedEventArgs> eventHandler = (__, ___) =>
                {
                    if (condition.Value)
                        tokenSource.Cancel();
                };

                condition.ValueChanged += eventHandler;

                Stopwatch timer = Stopwatch.StartNew();
                await Task.Delay(timeout, tokenSource.Token);
                timer.Stop();

                condition.ValueChanged -= eventHandler;

                result = timer.Elapsed.TotalMilliseconds <= timeout;
            }

            return result;
        }

        /// <summary>
        /// Wait for a specific amount of time
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="timeToWait">The time to wait (as <see cref="TimeSpan"/>)</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this object _, TimeSpan timeToWait)
            => await Task.Delay(timeToWait);

        /// <summary>
        /// Wait for a <see cref="Task"/> to complete without a timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="t">The <see cref="Task"/> to wait</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this object _, Task t)
            => await t;

        /// <summary>
        /// Wait for a <see cref="Task"/> to complete with timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="t">The <see cref="Task"/> to wait</param>
        /// <param name="timeout">The timeout (in milliseconds)</param>
        /// <returns>
        /// The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if the <see cref="Task"/> completed,
        /// <see langword="false"/> if timeout occurred)
        /// </returns>
        public static async Task<bool> WaitFor(this object _, Task t, int timeout)
            => await t.StartWithTimeout(timeout);

        /// <summary>
        /// Wait for a <see cref="Task"/> to complete with timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="t">The <see cref="Task"/> to wait</param>
        /// <param name="timeout">The timeout (as <see cref="TimeSpan"/>)</param>
        /// <returns>
        /// The (async) <see cref="Task{TResult}"/> (<see langword="true"/> if the <see cref="Task"/> completed,
        /// <see langword="false"/> if timeout occurred)
        /// </returns>
        public static async Task<bool> WaitFor(this object _, Task t, TimeSpan timeout)
            => await _.WaitFor(t, timeout.Milliseconds);
    }
}