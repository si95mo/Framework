using Core;
using Core.Conditions;
using Core.Parameters;
using System;
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
    }
}