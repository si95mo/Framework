using Hardware;
using MathNet.Filtering;

namespace Signal.Processing
{
    /// <summary>
    /// Implement a class with filtering functionalities
    /// </summary>
    public class Filter
    {
        private OnlineFilter filter;

        private AnalogOutput output;

        /// <summary>
        /// The <see cref="Filter"/> output as an <see cref="AnalogOutput"/>
        /// </summary>
        /// <remarks>
        /// This property will only be valorized if the instance of <see cref="Filter"/> has been created with
        /// <see cref="Filter(double, double, Channel{double})"/> or <see cref="Filter(double, double, double, Channel{double})"/> <br/><br/>
        /// The effects of this method are equivalent to the ones of using <see cref="FilterSamples(double[])"/>,
        /// but the filtering action is not done offline in this case!
        /// </remarks>
        public AnalogOutput Output => output;

        /// <summary>
        /// Create a new instance of low-pass <see cref="Filter"/>
        /// </summary>
        /// <param name="cutoffFrequence">The cutoff frequency (in Hertz)</param>
        /// <param name="sampleRate">The sample rate (in samples per second)</param>
        public Filter(double cutoffFrequence, double sampleRate)
        {
            filter = OnlineFilter.CreateLowpass(ImpulseResponse.Finite, sampleRate, cutoffFrequence);
        }

        /// <summary>
        /// Create a new instance of a low-pass filter <see cref="Filter"/> that processes each sample acquired
        /// </summary>
        /// <param name="cutoffFrequence">The cutoff frequency (in Hertz)</param>
        /// <param name="sampleRate">The sample rate (in samples per second)</param>
        /// <param name="input">The <see cref="Channel{T}"/> containing the samples to filter</param>
        public Filter(double cutoffFrequence, double sampleRate, Channel<double> input) : this(cutoffFrequence, sampleRate)
        {
            output = new AnalogOutput($"{input.Code}.Filtered", input.MeasureUnit, input.Format);

            input.ValueChanged += (object _, Core.ValueChangedEventArgs __) =>
                output.Value = filter.ProcessSample(input.Value);
        }

        /// <summary>
        /// Create a new instance of a band-pass <see cref="Filter"/>
        /// </summary>
        /// <param name="lowpassFrequency">The lowpass frequency (in Hertz)</param>
        /// <param name="highpassFrequency">The highpass frequency (in Hertz)</param>
        /// <param name="sampleRate">The sample rate (in samples per second)</param>
        public Filter(double lowpassFrequency, double highpassFrequency, double sampleRate)
        {
            filter = OnlineFilter.CreateBandpass(ImpulseResponse.Finite, sampleRate, lowpassFrequency, highpassFrequency);
        }

        /// <summary>
        /// Create a new instance of a band-pass <see cref="Filter"/> that processes each sample acquired
        /// </summary>
        /// <param name="lowpassFrequency">The lowpass frequency (in Hertz)</param>
        /// <param name="highpassFrequency">The highpass frequency (in Hertz)</param>
        /// <param name="sampleRate">The sample rate (in samples per second)</param>
        /// <param name="input">The <see cref="Channel{T}"/> containing the samples to filter</param>
        public Filter(double lowpassFrequency, double highpassFrequency, double sampleRate, Channel<double> input)
            : this(lowpassFrequency, highpassFrequency, sampleRate)
        {
            output = new AnalogOutput($"{input.Code}.Filtered", input.MeasureUnit, input.Format);

            input.ValueChanged += (object _, Core.ValueChangedEventArgs __) =>
                output.Value = filter.ProcessSample(input.Value);
        }

        /// <summary>
        /// Filter the data acquired
        /// </summary>
        /// <remarks>
        /// The effects of this method are equivalent to the ones of using <see cref="Output"/>,
        /// but the filtering action is not done online in this case!
        /// </remarks>
        /// <param name="samples">The samples to filter</param>
        /// <returns>The filtered samples</returns>
        public double[] FilterSamples(double[] samples)
        {
            double[] filteredSamples = filter.ProcessSamples(samples);
            return filteredSamples;
        }
    }
}