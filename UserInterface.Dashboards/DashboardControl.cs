using Hardware;
using Newtonsoft.Json;
using System;
using System.Drawing;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Handle an <see cref="IDashboardControl"/> serialization
    /// </summary>
    internal class DashboardControl
    {
        /// <summary>
        /// The <see cref="IChannel"/>
        /// </summary>
        [JsonProperty]
        public string ChannelCode { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        [JsonProperty]
        public string Description { get; set; }

        /// <summary>
        /// The <see cref="IDashboardControl"/> <see cref="System.Type"/>
        /// </summary>
        [JsonProperty]
        public Type Type { get; set; }

        /// <summary>
        /// The <see cref="IDashboardControl"/> <see cref="System.Drawing.Size"/>
        /// </summary>
        public Size Size { get; set; }

        /// <summary>
        /// The <see cref="IDashboardControl"/> location (<see cref="Point"/>)
        /// </summary>
        public Point Location { get; set; }

        /// <summary>
        /// Create a new instance of <see cref="DashboardControl"/>
        /// </summary>
        /// <param name="channelCode">The channel code</param>
        /// <param name="description">The channel description</param>
        /// <param name="type">The <see cref="IDashboardControl"/> <see cref="System.Type"/></param>
        /// <param name="size">The <see cref="IDashboardControl"/> <see cref="System.Drawing.Size"/></param>
        /// <param name="location">The <see cref="IDashboardControl"/> location (<see cref="Point"/>)</param>
        public DashboardControl(string channelCode, string description, Type type, Size size, Point location)
        {
            ChannelCode = channelCode;
            Description = description;
            Type = type;
            Size = size;
            Location = location;
        }
    }
}