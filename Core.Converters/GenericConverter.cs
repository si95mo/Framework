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
        /// The <see cref="GenericConverter{TIn, TOut}"/> <see cref="Func{T, TResult}"/>
        /// used in conversion
        /// </summary>
        public override Func<TIn, TOut> Converter
        {
            get => converter;
            set => converter = value;
        }

        /// <summary>
        /// Initialize a new instance of <see cref="GenericConverter{TIn, TOut}"/>
        /// </summary>
        /// <param name="converter">The conversion <see cref="Func{T, TResult}"/></param>
        public GenericConverter(Func<TIn, TOut> converter) : base()
        {
            this.converter = converter;
        }
    }
}