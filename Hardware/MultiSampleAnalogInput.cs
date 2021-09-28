using System;
using System.Linq;

namespace Hardware
{
    public class MultiSampleAnalogInput : Channel<double[]>
    {
        /// <summary>
        /// Create a new instance of <see cref="MultiSampleAnalogInput"/>
        /// </summary>
        /// <param name="code">The code</param>
        public MultiSampleAnalogInput(string code, string measureUnit = "", string format = "") : base(code)
        {
            value = default;
            this.measureUnit = measureUnit;
            this.format = format;
        }

        /// <summary>
        /// Create a new instance of <see cref="MultiSampleAnalogInput"/>
        /// </summary>
        public MultiSampleAnalogInput() : this(Guid.NewGuid().ToString())
        { }

        /// <summary>
        /// Give a textual description of the <see cref="Channel{T}"/>
        /// </summary>
        /// <returns>The textual description</returns>
        public override string ToString()
        {
            double meanValue = value.ToList().Average();
            string description = $"{meanValue.ToString(format)}{measureUnit}";

            return description;
        }
    }
}