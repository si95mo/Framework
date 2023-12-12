using Core;
using Core.Converters;
using Core.Parameters;
using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Extensions
{
    /// <summary>
    /// Provides framework-related extension methods
    /// </summary>
    public static class FrameworkExtension
    {
        #region Parameters

        /// <summary>
        /// Set a description to the <paramref name="source"/> <see cref="IParameter"/>
        /// </summary>
        /// <param name="source">The source <see cref="IParameter"/></param>
        /// <param name="description">The description</param>
        public static void WithDescription(IParameter source, string description)
            => source.Description = description;

        public static NumericParameter Resample(this NumericParameter source)
        {
            NumericParameter destination = new NumericParameter(Guid.NewGuid().ToString(), source.Value, source.MeasureUnit, source.Format);

            Stopwatch timer = Stopwatch.StartNew();
            source.ConnectTo(
                destination,
                new GenericConverter<double, double>((x) =>
                    {
                        double y = timer.Elapsed.TotalMilliseconds >= 200d ? x : destination.Value; // 5Hz of refresh rate
                        return y;
                    }
                )
            );

            return destination;
        }

        #region Wrappers

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

        #endregion Wrappers

        #endregion Parameters

        #region Generic WaitFor

        /// <summary>
        /// Wait for a specific amount of time
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="timeToWait">The time to wait (as <see cref="TimeSpan"/>)</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this IProperty _, TimeSpan timeToWait)
            => await Task.Delay(timeToWait);

        /// <summary>
        /// Wait for a <see cref="Task"/> to complete without a timeout
        /// </summary>
        /// <param name="_">The source</param>
        /// <param name="t">The <see cref="Task"/> to wait</param>
        /// <returns>The (async) <see cref="Task"/></returns>
        public static async Task WaitFor(this IProperty _, Task t)
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
        public static async Task<bool> WaitFor(this IProperty _, Task t, int timeout)
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
        public static async Task<bool> WaitFor(this IProperty _, Task t, TimeSpan timeout)
            => await _.WaitFor(t, timeout.Milliseconds);

        /// <summary>
        /// Wait for a <see cref="Task{TResult}"/> to complete
        /// </summary>
        /// <typeparam name="T">The type of the <see cref="Task{TResult}"/> result</typeparam>
        /// <param name="_">The source</param>
        /// <param name="t">The <see cref="Task{TResult}"/> to wait</param>
        /// <returns>The (async) <see cref="Task{TResult}"/></returns>
        public static async Task<T> WaitFor<T>(this IProperty _, Task<T> t)
            => await t;

        #endregion Generic WaitFor
    }
}