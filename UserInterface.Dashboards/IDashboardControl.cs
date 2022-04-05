using Hardware;

namespace UserInterface.Dashboards
{
    /// <summary>
    /// Describe the prototype of a dashboard control
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal interface IDashboardControl<T>
    {
        /// <summary>
        /// The <see cref="IDashboardControl{T}"/> <see cref="IChannel"/>
        /// </summary>
        IChannel Channel { get; }

        /// <summary>
        /// The <see cref="IDashboardControl{T}"/> value
        /// </summary>
        T Value { get; }

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
}
