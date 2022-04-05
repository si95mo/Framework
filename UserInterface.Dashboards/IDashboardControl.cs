using Hardware;

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
        IChannel Channel { get; }

        /// <summary>
        /// The description
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// Set the <see cref="IDashboardControl{T}"/> <see cref="Channel"/>
        /// </summary>
        /// <param name="channel">The <see cref="IChannel"/> to set</param>
        void SetChannel(IChannel channel);

        /// <summary>
        /// Set the <see cref="IDashboardControl{T}"/> <see cref="Channel"/>
        /// </summary>
        /// <param name="channeCode">The <see cref="IChannel"/> to set code</param>
        void SetChannel(string channeCode);
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