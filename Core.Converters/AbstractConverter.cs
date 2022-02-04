using System;

namespace Core.Converters
{
    /// <summary>
    /// Define a basic structure for a converter
    /// </summary>
    /// <typeparam name="TIn">The input type of the value to convert</typeparam>
    /// <typeparam name="TOut">The output type of the conversion</typeparam>
    public abstract class AbstractConverter<TIn, TOut> : IConverter<TIn, TOut>
    {
        protected Func<TIn, TOut> converter;
        protected IProperty<TIn> sourceParameter;
        protected IProperty<TOut> destinationParameter;

        /// <summary>
        /// The <see cref="AbstractConverter{TIn, TOut}"/> <see cref="Func{T, TResult}"/>
        /// used in conversion
        /// </summary>
        public virtual Func<TIn, TOut> Converter
        {
            get => converter;
            set => converter = value;
        }

        /// <summary>
        /// Create a new isntance of <see cref="AbstractConverter{TIn, TOut}"/>
        /// </summary>
        protected AbstractConverter()
        {
            sourceParameter = null;
            destinationParameter = null;
        }

        /// <summary>
        /// Execute the conversion
        /// </summary>
        /// <param name="arg">The argument to convert</param>
        /// <returns>The result of the conversion</returns>
        public virtual TOut Execute(TIn arg) => converter.Invoke(arg);

        /// <summary>
        /// Connects an <see cref="IProperty"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IProperty"/></param>
        /// <param name="destinationParameter">The destination <see cref="IProperty"/></param>
        public virtual void Connect(IProperty sourceParameter, IProperty destinationParameter)
        {
            this.sourceParameter = sourceParameter as IProperty<TIn>;
            this.destinationParameter = destinationParameter as IProperty<TOut>;

            (sourceParameter as IProperty<TIn>).ConnectTo(this.sourceParameter);
            this.destinationParameter.ConnectTo(destinationParameter as IProperty<TOut>);

            this.destinationParameter.Value =
                converter.Invoke((sourceParameter as IProperty<TIn>).Value);

            this.sourceParameter.ValueChanged += PropagateValues;
        }

        /// <summary>
        /// Connect two objects in order to propagate the converted value
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            destinationParameter.Value = converter.Invoke(sourceParameter.Value);
        }
    }
}