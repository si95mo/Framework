using Alarms;
using Core.Conditions;
using Extensions;

namespace Tasks
{
    /// <summary>
    /// Define a <see cref="IAwaitable"/> task with a predefined <see cref="Alarm"/> logic
    /// </summary>
    public abstract class AwaitableWithAlarm : Awaitable
    {
        private DummyCondition condition;

        /// <summary>
        /// Create a new instance of <see cref="AwaitableWithAlarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        public AwaitableWithAlarm(string code) : base(code)
        {
            condition = new DummyCondition($"{Code}.AlarmCondition", false);

            Alarm = Alarm.New($"{Code}.Alarm", this, "Alarm fired", condition.IsTrue());
            Alarm.OnFire(() => Fail(Alarm.Message));
        }

        /// <summary>
        /// Fire the <see cref="Alarm"/>
        /// </summary>
        protected void FireAlarm()
            => condition.Force(true);

        /// <summary>
        /// Fire the <see cref="Alarm"/> with a custom <paramref name="message"/>
        /// </summary>
        /// <param name="message">The message</param>
        protected void FireAlarm(string message)
        {
            Alarm.Message = message;
            condition.Force(true);
        }

        /// <summary>
        /// Reset the <see cref="Alarm"/>
        /// </summary>
        protected void ResetAlarm()
        => condition.Force(false);
    }
}
