using System;

namespace Core.Converters
{
    /// <summary>
    /// Define a generic converter
    /// </summary>
    /// <typeparam name="TIn">The input type</typeparam>
    /// <typeparam name="TOut">The output type</typeparam>
    public interface IConverter<TIn, TOut> : IConverter
    {
        /// <summary>
        /// The option to trigger the conversion on value change or value set events
        /// </summary>
        bool OnValueChange { get; }

        /// <summary>
        /// The <see cref="IConverter"/> <see cref="Func{T, TResult}"/>
        /// used in the conversion
        /// </summary>
        Func<TIn, TOut> Converter { get; set; }
    }
}