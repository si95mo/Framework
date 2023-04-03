using Hardware;
using System;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Basic prototype of a dashboard control
    /// </summary>
    internal interface IDashboardControl
    {
        /// <summary>
        /// The <see cref="IDashboardControl{T}"/> <see cref="IChannel"/>
        /// </summary>
        [JsonProperty]
        IChannel Channel { get; set; }

        /// <summary>
        /// The related <see cref="IChannel"/> code
        /// </summary>
        [JsonProperty]
        string ChannelCode { get; set; }

        /// <summary>
        /// The description
        /// </summary>
        [JsonProperty]
        string Description { get; set; }

        /// <summary>
        /// The <see cref="IDashboardControl"/> <see cref="System.Type"/>
        /// </summary>
        [JsonProperty]
        Type Type { get; set; }

        /// <summary>
        /// Set the <see cref="IDashboardControl{T}"/> <see cref="Channel"/>
        /// </summary>
        /// <param name="channel">The <see cref="IChannel"/> to set</param>
        void SetChannel(IChannel channel);

        /// <summary>
        /// Set the <see cref="IDashboardControl{T}"/> <see cref="Channel"/>
        /// </summary>
        /// <param name="channelCode">The <see cref="IChannel"/> to set code</param>
        void SetChannel(string channelCode);
    }

    /// <summary>
    /// Describe the prototype of a dashboard control with a value
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="IDashboardControl"/></typeparam>
    internal interface IDashboardControl<T> : IDashboardControl
    {
        /// <summary>
        /// The <see cref="IDashboardControl{T}"/> value
        /// </summary>
        T Value { get; }
    }
}