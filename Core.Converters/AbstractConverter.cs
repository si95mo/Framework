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
        protected IProperty<TIn> sourceParameter;
        protected IProperty<TOut> destinationParameter;

        public bool OnValueChange { get; protected set; }

        /// <summary>
        /// The <see cref="AbstractConverter{TIn, TOut}"/> <see cref="Func{T, TResult}"/>
        /// used in conversion
        /// </summary>
        public virtual Func<TIn, TOut> Converter { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="AbstractConverter{TIn, TOut}"/>
        /// </summary>
        protected AbstractConverter(bool onValueChange)
        {
            sourceParameter = null;
            destinationParameter = null;

            OnValueChange = onValueChange;
        }

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

            this.destinationParameter.Value = Converter.Invoke((sourceParameter as IProperty<TIn>).Value);

            if (OnValueChange)
            {
                this.sourceParameter.ValueChanged += SourceParameter_ValueChanged;

            }
            else
            {
                this.sourceParameter.ValueSet += SourceParameter_ValueSet;
            }
        }

        /// <summary>
        /// Connect two objects in order to propagate the converted value
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void SourceParameter_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            destinationParameter.Value = Converter.Invoke(sourceParameter.Value);
        }

        private void SourceParameter_ValueSet(object sender, ValueSetEventArgs e)
        {
            destinationParameter.Value = Converter.Invoke(sourceParameter.Value);
        }
    }
}