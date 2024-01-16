using Core;
using System;
using System.Linq;

namespace Hardware
{
    /// <summary>
    /// Implement a multi sample analo input
    /// </summary>
    public class MultiSampleAnalogInput : Channel<double[]>, IReadOnlyProperty<double[]>
    {
        private double[] value;

        /// <summary>
        /// The <see cref="Channel{T}"/> value
        /// </summary>
        public override double[] Value
        {
            get => value;
            set
            {
                if (!value.Equals(this.value))
                {
                    double[] oldValue = this.value;
                    this.value = new double[value.Length];
                    Array.Copy(value, this.value, value.Length);
                    OnValueChanged(new ValueChangedEventArgs(oldValue, this.value));
                }
            }
        }

        /// <summary>
        /// Create a new instance of <see cref="MultiSampleAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public MultiSampleAnalogInput(string code, string measureUnit = "", string format = "0.0") : base(code, measureUnit, format)
        {
            Initialize();
        }

        /// <summary>
        /// Create a new instance of <see cref="MultiSampleAnalogInput"/>
        /// </summary>
        public MultiSampleAnalogInput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Create a new instance of <see cref="MultiSampleAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="resource">The <see cref="IResource"/></param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        public MultiSampleAnalogInput(string code, IResource resource, string measureUnit = "", string format = "0.0") : base(code, measureUnit, format, resource)
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the attributes
        /// </summary>
        private void Initialize()
        {
            ChannelType = ChannelType.MultiSampleAnalogInput;
            value = new double[] { 0d };
        }

        /// <summary>
        /// Give a textual description of the <see cref="Channel{T}"/>
        /// </summary>
        /// <returns>The textual description</returns>
        public override string ToString()
        {
            double meanValue = value.ToList().Average();
            string description = $"{meanValue.ToString(Format)}{MeasureUnit}";

            return description;
        }
    }
}