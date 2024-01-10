using System;

namespace Core.Converters
{
    /// <summary>
    /// Describe a generic converter
    /// </summary>
    /// <typeparam name="TIn">The type of the input</typeparam>
    /// <typeparam name="TOut">The type of the output</typeparam>
    public class GenericConverter<TIn, TOut> : AbstractConverter<TIn, TOut>
    {
        /// <summary>
        /// Initialize a new instance of <see cref="GenericConverter{TIn, TOut}"/>
        /// </summary>
        /// <param name="converter">The conversion <see cref="Func{T, TResult}"/></param>
        /// <param name="onValueChange">The option to trigger the update on value change or set event</param>
        public GenericConverter(Func<TIn, TOut> converter, bool onValueChange = true) : base(onValueChange)
        {
            Converter = converter;
        }
    }
}