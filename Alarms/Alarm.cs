using Core;
using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
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
        private IProperty source;
        private ICondition firingCondition;
        private Action onFireAction;

        public string Code { get; private set; }
        public object ValueAsObject { get => Code; set => _ = value; }
        public Type Type => GetType();
        public string SourceCode => source.Code;
        public string Message { get; set; }
        public DateTime FiringTime { get; private set; }
        public BoolParameter Fired { get; private set; }

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public Alarm(string code, string sourceCode, string message, ICondition firingCondition)
        {
            Initialize(code, message, firingCondition);
            source = ServiceBroker.Get<IProperty>().Get(sourceCode);
        }

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public Alarm(string code, IProperty source, string message, ICondition firingCondition)
        {
            Initialize(code, message, firingCondition);
            this.source = source;
        }

        #endregion Constructors

        #region IAlarm implementation

        public void Fire()
        {
            Fired.Value = true;
            FiringTime = DateTime.Now;

            onFireAction?.Invoke();

            // Stop the source if possible
            if (source is IResource)
                (source as IResource).Stop();
            else if (source is IDevice)
                (source as IDevice).Stop();
        }

        public void Reset()
        {
            Fired.Value = false;
            FiringTime = default;
        }

        #endregion IAlarm implementation

        /// <summary>
        /// Define the <see cref="Action"/> to invoke in case of <see cref="Fire"/>
        /// </summary>
        /// <param name="onFireAction"></param>
        public void OnFire(Action onFireAction)
            => this.onFireAction = onFireAction;

        #region Factory methods

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Alarm New(string code, string sourceCode, string message, ICondition firingCondition)
            => new Alarm(code, sourceCode, message, firingCondition);

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Alarm New(string code, IProperty source, string message, ICondition firingCondition)
            => new Alarm(code, source, message, firingCondition);

        #endregion Factory methods

        #region Private methods

        /// <summary>
        /// Initialize <see cref="Alarm"/> attributes
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The firing <see cref="ICondition"/></param>
        private void Initialize(string code, string message, ICondition firingCondition)
        {
            Code = code;
            Message = message;

            Fired = new BoolParameter($"{Code}.{nameof(Fired)}", false);

            this.firingCondition = firingCondition;

            onFireAction = null;

            this.firingCondition.ValueChanged += FiringCondition_ValueChanged;
        }

        private void FiringCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValueAsBool)
                Fire();
        }

        #endregion Private methods
    }
}