﻿using System;

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
        /// The <see cref="IConverter"/> <see cref="Func{T, TResult}"/>
        /// used in the conversion
        /// </summary>
        Func<TIn, TOut> Converter
        { get; set; }

        /// <summary>
        /// Execute the conversion
        /// </summary>
        /// <param name="arg">The argument to convert</param>
        /// <returns>The result of the conversion</returns>
        TOut Execute(TIn arg);
    }
}