using Core;
using Core.Conditions;
using System;

namespace Alarms
{
    public class Alarm : IProperty, IAlarm
    {
        private string code;
        private string source;
        private string message;
        private DateTime firingTime;

        public string Code => code;

        public object ValueAsObject { get => Code; set => _ = value; }

        public Type Type => GetType();

        public string Source => source;

        public string Message => message;

        public DateTime FiringTime => firingTime;

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        public Alarm(string code, string source, string message, ICondition firingCondition)
        {
            this.code = code;
            this.source = source;
            this.message = message;

            firingCondition.ValueChanged += FiringCondition_ValueChanged;
        }

        private void FiringCondition_ValueChanged(object sender, ValueChangedEventArgs e)
            => Fire();

        public void Fire()
        {
            firingTime = DateTime.Now;
        }
    }
}
