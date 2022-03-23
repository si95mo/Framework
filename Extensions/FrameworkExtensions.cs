using Core.Parameters;
using System;

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
    }
}