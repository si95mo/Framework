using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Core.Parameters
{
    /// <summary>
    /// Define a ring buffer for <see cref="IProperty{T}"/>
    /// </summary>
    /// <typeparam name="TProperty">The type of the <see cref="IProperty{T}"/></typeparam>
    /// <typeparam name="TValue">The type of the value of the <see cref="IProperty{T}"/></typeparam>
    public class RingBufferParameter<TProperty, TValue> : Parameter<TValue[]> where TProperty : IProperty<TValue>
    {
        /// <summary>
        /// The property to buffer
        /// </summary>
        public TProperty Property { get; private set; }

        /// <summary>
        /// The buffer size
        /// </summary>
        public int BufferSize { get; private set; }

        /// <summary>
        /// The debounce <see cref="TimeSpan"/>
        /// </summary>
        public TimeSpan DebounceTime => debounceTime;

        /// <summary>
        /// The sampling frequency (in Hz)
        /// </summary>
        public double SamplingFrequency { get; private set; }

        /// <summary>
        /// The index of the last added item in the buffer
        /// </summary>
        public NumericParameter LastAddedItemIndex { get; private set; }

        /// <summary>
        /// The <see cref="EventHandler{TEventArgs}"/> of <see cref="ValueChangedEventArgs"/> for when the circular buffer is full 
        /// (i.e. a cycle done, starting to overwrite the oldest added elements)
        /// </summary>
        /// <remarks>
        /// In this case <see cref="ValueChangedEventArgs.OldValue"/> will be <see cref="EventArgs.Empty"/>. The actual <see cref="RingBufferParameter{TP, TV}"/>
        /// value will be stored in <see cref="ValueChangedEventArgs.NewValue"/>
        /// </remarks>
        public event EventHandler<ValueChangedEventArgs> BufferFull;

        private readonly TimeSpan debounceTime;
        private readonly Debouncer debouncer;
        private readonly Queue<double> times;

        private Stopwatch timer;

        /// <summary>
        /// Create a new instance of <see cref="RingBufferParameter{TP, TV}"/>
        /// </summary>
        /// <remarks>Set <paramref name="debounceTimeInMilliseconds"/> to 0 to avoid the <see cref="Debouncer"/></remarks>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty"/></param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="debounceTimeInMilliseconds">The debounce time in milliseconds</param>
        public RingBufferParameter(string code, TProperty property, int bufferSize, double debounceTimeInMilliseconds = 0d, string measureUnit = "", string format = "0.000")
            : this(code, property, bufferSize, TimeSpan.FromMilliseconds(debounceTimeInMilliseconds), measureUnit, format)
        { }

        /// <summary>
        /// Create a new instance of <see cref="RingBufferParameter{TP, TV}"/>
        /// </summary>
        /// <remarks>Set <paramref name="debounceTime"/> to <see cref="TimeSpan.Zero"/> to avoid the <see cref="Debouncer"/></remarks>
        /// <param name="code">The code</param>
        /// <param name="property">The <see cref="IProperty"/></param>
        /// <param name="bufferSize">The buffer size</param>
        /// <param name="debounceTime">The debounce <see cref="TimeSpan"/></param>
        public RingBufferParameter(string code, TProperty property, int bufferSize, TimeSpan debounceTime, string measureUnit = "", string format = "0.000") : base(code)
        {
            MeasureUnit = measureUnit;
            Format = format;

            Property = property;
            BufferSize = bufferSize;

            Value = new TValue[bufferSize];
            LastAddedItemIndex = new NumericParameter($"{Code}.{nameof(LastAddedItemIndex)}", measureUnit: "#", format: "0", value: 0d);

            times = new Queue<double>();

            this.debounceTime = debounceTime;

            debouncer = new Debouncer(debounceTime);
            timer = Stopwatch.StartNew();
            property.ValueChanged += Property_ValueChanged;
        }

        private void Property_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (debounceTime > TimeSpan.Zero)
            {
                debouncer.Invoke(() => UpdateBuffer(Property));
            }
            else
            {
                UpdateBuffer(Property);
            }
        }

        private void UpdateBuffer(TProperty item)
        {
            timer.Stop();

            // Simple moving average for the sampling frequency
            if (times.Count > 16)
            {
                times.Dequeue();
            }
            times.Enqueue(1d / timer.Elapsed.TotalSeconds);
            SamplingFrequency = times.Average();

            Value[(int)(LastAddedItemIndex.Value++ % BufferSize)] = item.Value;

            if (LastAddedItemIndex.ValueAsInt == BufferSize)
            {
                LastAddedItemIndex.Value = 0d;
                BufferFull?.Invoke(this, new ValueChangedEventArgs(EventArgs.Empty, Value));
            }
        }

        public override string ToString()
        {
            string description = $"{Value[0]}{MeasureUnit}";
            return description;
        }
    }
}
