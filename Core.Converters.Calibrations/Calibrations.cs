using Core.DataStructures;
using Hardware;

namespace Core.Converters.Calibrations
{
    /// <summary>
    /// Static class that performs various type of calibrations between channels
    /// </summary>
    public static class Calibrations
    {
        /// <summary>
        /// Create a calibrated <see cref="AnalogInput"/> based on the <paramref name="rawChannel"/> <see cref="AnalogInput"/>
        /// </summary>
        /// <remarks>
        /// The calibrated channel is automatically added to the <see cref="ServiceBroker"/>
        /// </remarks>
        /// <param name="rawChannel">The raw <see cref="AnalogInput"/></param>
        /// <param name="x0">The <paramref name="rawChannel"/> first point</param>
        /// <param name="x1">The <paramref name="rawChannel"/> second point</param>
        /// <param name="y0">The calibrated channel first point</param>
        /// <param name="y1">The calibrated channel second point</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <returns>The calibrated <see cref="AnalogInput"/></returns>
        public static AnalogInput CreateCalibratedChannel(AnalogInput rawChannel, double x0, double x1, double y0, double y1,
            string measureUnit = "", string format = "0.0")
        {
            AnalogInput calibratedChannel = new AnalogInput(rawChannel.Code + "_Calibrated", measureUnit, format);
            rawChannel.ConnectTo(calibratedChannel, new LinearInterpolationConverter(x0, x1, y0, y1));

            ServiceBroker.Add<IChannel>(calibratedChannel);

            return calibratedChannel;
        }

        /// <summary>
        /// Create a calibrated <see cref="AnalogOutput"/> based on the <paramref name="rawChannel"/> <see cref="AnalogOutput"/>
        /// </summary>
        /// <remarks>
        /// The calibrated channel is automatically added to the <see cref="ServiceBroker"/>
        /// </remarks>
        /// <param name="rawChannel">The raw <see cref="AnalogOutput"/></param>
        /// <param name="x0">The <paramref name="rawChannel"/> first point</param>
        /// <param name="x1">The <paramref name="rawChannel"/> second point</param>
        /// <param name="y0">The calibrated channel first point</param>
        /// <param name="y1">The calibrated channel second point</param>
        /// <param name="measureUnit">The measure unit</param>
        /// <param name="format">The format</param>
        /// <returns>The calibrated <see cref="AnalogOutput"/></returns>
        public static AnalogOutput CreateCalibratedChannel(AnalogOutput rawChannel, double x0, double x1, double y0, double y1,
            string measureUnit = "", string format = "0.0")
        {
            AnalogOutput calibratedChannel = new AnalogOutput(rawChannel.Code + "_Calibrated", measureUnit, format);
            calibratedChannel.ConnectTo(rawChannel, new LinearInterpolationConverter(x0, x1, y0, y1));

            ServiceBroker.Add<IChannel>(calibratedChannel);

            return calibratedChannel;
        }
    }
}