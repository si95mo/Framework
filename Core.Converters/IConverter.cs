using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Converters
{
    public interface IConverter<TIn, TOut>
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

        /// <summary>
        /// Connect two <see cref="IProperty"/> in order to 
        /// propagate the converted value
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IParameter"/></param>
        /// <param name="destinationParameter">The destination <see cref="IParameter"/></param>
        void Connect(IProperty<TIn> sourceParameter, IProperty<TOut> destinationParameter);
    }
}
