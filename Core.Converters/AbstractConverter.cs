using System;

namespace Core.Converters
{
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
        /// Connects an <see cref="IParameter"/> to another
        /// in order to propagate its value;
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IParameter"/></param>
        /// <param name="destinationParameter">The destination <see cref="IParameter"/></param>
        public virtual void Connect(IProperty<TIn> sourceParameter, IProperty<TOut> destinationParameter)
        {
            this.sourceParameter = sourceParameter;
            this.destinationParameter = destinationParameter;

            sourceParameter.ConnectTo(this.sourceParameter as IProperty<TIn>);
            this.destinationParameter.ConnectTo(destinationParameter as IProperty<TOut>);

            this.sourceParameter.ValueChanged += PropagateValues;
        }

        /// <summary>
        /// Connect two <see cref="IParameter"/> in order to
        /// propagate the converted value
        /// </summary>
        /// <param name="sourceParameter">The source <see cref="IParameter"/></param>
        /// <param name="destinationParameter">The destination <see cref="IParameter"/></param>
        protected virtual void PropagateValues(object sender, ValueChangedEventArgs e)
        {
            destinationParameter.Value = converter.Invoke(sourceParameter.Value);
        }
    }
}