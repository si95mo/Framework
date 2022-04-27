using Core;
using Core.Conditions;
using Core.DataStructures;
using Devices;
using Hardware;
using System;

namespace Alarms
{
    /// <summary>
    /// Implement the <see cref="IAlarm"/> interface
    /// </summary>
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
        {
            if ((bool)e.NewValue)
                Fire();
        }

        public void Fire()
        {
            firingTime = DateTime.Now;

            // Stop the alarm source (if possible)
            IProperty alarmSource = ServiceBroker.Get<IProperty>().Get(source);
            if (alarmSource is IResource)
                (alarmSource as IResource).Stop();
            else
            {
                if (alarmSource is IDevice)
                    (alarmSource as IDevice).Stop();
            }
        }

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Alarm New(string code, string source, string message, ICondition firingCondition)
            => new Alarm(code, source, message, firingCondition);
    }
}