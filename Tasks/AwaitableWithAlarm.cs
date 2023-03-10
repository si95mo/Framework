using DiagnosticMessages;
using Core.Conditions;

namespace Tasks
{
    /// <summary>
    /// Define a <see cref="IAwaitable"/> task with a predefined <see cref="Alarm"/> logic
    /// </summary>
    public abstract class AwaitableWithAlarm : Awaitable
    {
        private DummyCondition alarmCondition, warnCondition;

        /// <summary>
        /// Create a new instance of <see cref="AwaitableWithAlarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="scheduler">The <see cref="Scheduler"/> to use</param>
        public AwaitableWithAlarm(string code, Scheduler scheduler = null) : base(code, scheduler)
        {
            alarmCondition = new DummyCondition($"{Code}.AlarmCondition", false);
            warnCondition = new DummyCondition($"{Code}.WarnCondition", false);

            Alarm = Alarm.New($"{Code}.Alarm", this, "Alarm fired", alarmCondition.IsTrue());
            Alarm.OnFire(() => Fail(Alarm.Message));

            Warn = Warn.New($"{Code}.Warn", this, "Warn fires", warnCondition.IsTrue());
            Warn.OnFire(() => Fail(Warn.Message));
        }

        #region Alarm

        /// <summary>
        /// Fire the <see cref="Alarm"/>
        /// </summary>
        protected void FireAlarm()
            => alarmCondition.Force(true);

        /// <summary>
        /// Fire the <see cref="Alarm"/> with a custom <paramref name="message"/>
        /// </summary>
        /// <param name="message">The message</param>
        protected void FireAlarm(string message)
        {
            Alarm.Message = message;
            alarmCondition.Force(true);
        }

        /// <summary>
        /// Reset the <see cref="Alarm"/>
        /// </summary>
        protected void ResetAlarm()
        => alarmCondition.Force(false);

        #endregion Alarm

        #region Warn

        /// <summary>
        /// Fire the <see cref="Warn"/>
        /// </summary>
        protected void FireWarn()
            => warnCondition.Force(true);

        /// <summary>
        /// Fire the <see cref="Warn"/> with a custom <paramref name="message"/>
        /// </summary>
        /// <param name="message">The message</param>
        protected void FireWarn(string message)
        {
            Warn.Message = message;
            warnCondition.Force(true);
        }

        /// <summary>
        /// Reset the <see cref="Warn"/>
        /// </summary>
        protected void ResetWarn()
        => warnCondition.Force(false);

        #endregion Warn
    }
}