using System;

namespace Alarms
{
    /// <summary>
    /// Define a basic alarm interface
    /// </summary>
    public interface IAlarm
    {
        /// <summary>
        /// The source that caused the <see cref="IAlarm"/> to <see cref="Fire"/>
        /// </summary>
        string Source { get; }

        /// <summary>
        /// The <see cref="IAlarm"/> message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The <see cref="IAlarm"/> firing time (as <see cref="DateTime"/>
        /// </summary>
        DateTime FiringTime { get; }

        /// <summary>
        /// Fire the <see cref="IAlarm"/>
        /// </summary>
        void Fire();
    }
}
