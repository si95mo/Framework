using System;

namespace Core.Converters
{
    public class LinearInterpolationConverter : AbstractConverter<double, double>
    {
        /// <summary>
        /// The source parameter first value
        /// </summary>
        public double X0 { get; set; }

        /// <summary>
        /// The source parameter second value
        /// </summary>
        public double X1 { get; set; }

        /// <summary>
        /// The destination parameter relative first value
        /// </summary>
        public double Y0 { get; set; }

        /// <summary>
        /// The source parameter relative second value
        /// </summary>
        public double Y1 { get; set; }

        // (y - y0) / (x - x0) = (y1 - y0) / (x1 - x0) ->
        // -> y = y0 * (1 - (x - x0) / (x1 - x0)) + y1 * (1 - (x - x0) / (x1 - x0))
        // x.ConnectTo(y, LinearInterpolationConverter) -> x: source, y: destination

        /// <summary>
        /// Create a new instance of <see cref="LinearInterpolationConverter"/>
        /// </summary>
        /// <param name="x0">The source first value</param>
        /// <param name="x1">The source second value</param>
        /// <param name="y0">The destination first value</param>
        /// <param name="y1">The destination second value</param>
        public LinearInterpolationConverter(double x0, double x1, double y0, double y1) : base()
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;

            converter = InterpolateLinearly();
        }

        /// <summary>
        /// <see cref="Func{T, TResult}"/> that linearly interpolate a value
        /// </summary>
        /// <returns>The interpolated value</returns>
        private Func<double, double> InterpolateLinearly()
        {
            return (y) =>
            {
                double x = sourceParameter != null ? sourceParameter.Value : 0d;

                if ((X1 - X0) == 0)
                    y = (Y0 + Y1) / 2;
                else
                    y = Y0 + (x - X0) * (Y1 - Y0) / (X1 - X0);
                return y;
            };
        }
    }
}