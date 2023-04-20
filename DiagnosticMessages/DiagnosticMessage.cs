﻿using Core;
using Core.Conditions;
using Core.DataStructures;
using Core.Parameters;
using System;
using System.Diagnostics;

namespace DiagnosticMessages
{
    /// <summary>
    /// Define the <see cref="EventArgs"/> in case of a <see cref="DiagnosticMessage"/> fired event
    /// </summary>
    public class FiredEventArgs : EventArgs
    {
        /// <summary>
        /// The timestamp
        /// </summary>
        public DateTime Timestamp { get; private set; }

        /// <summary>
        /// Create a new instance of <see cref="FiredEventArgs"/>
        /// </summary>
        public FiredEventArgs() : this(DateTime.Now)
        { }

        /// <summary>
        /// Create a new instance of <see cref="FiredEventArgs"/>
        /// </summary>
        /// <param name="timestamp">The timestamp</param>
        public FiredEventArgs(DateTime timestamp)
        {
            Timestamp = timestamp;
        }
    }

    /// <summary>
    /// Define a basic (i.e. abstract) implementation of an <see cref="IDiagnosticMessage"/>
    /// </summary>
    public abstract class DiagnosticMessage : IDiagnosticMessage
    {
        protected IProperty Source;
        protected ICondition FiringCondition;
        protected Action OnFireAction;

        private object eventLock = new object();

        #region Public properties

        public string Code { get; private set; }
        public object ValueAsObject { get => Code; set => _ = value; }
        public Type Type => GetType();
        public string SourceCode => Source.Code;
        public string Message { get; set; }
        public DateTime FiringTime { get; protected set; }

        #endregion Public properties

        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public DiagnosticMessage(string code, string message, string sourceCode = null, ICondition firingCondition = null)
        {
            Initialize(code, message, firingCondition);
            Source = sourceCode != null ? ServiceBroker.Get<IProperty>().Get(sourceCode) : null;
        }

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public DiagnosticMessage(string code, string message, IProperty source = null, ICondition firingCondition = null)
        {
            Initialize(code, message, firingCondition);
            Source = source;
        }

        #endregion Constructors

        #region IDiagnosticMessage implementation

        public abstract void Fire();

        public virtual void Reset()
        {
            FiringTime = default;
        }

        #endregion IDiagnosticMessage implementation

        #region Public methods

        /// <summary>
        /// Define the <see cref="Action"/> to invoke in case of <see cref="Fire"/>
        /// </summary>
        /// <param name="onFireAction">The <see cref="Action"/> to invoke</param>
        public virtual void OnFire(Action onFireAction)
            => OnFireAction = onFireAction;

        #endregion Public methods

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
            FiringCondition = firingCondition;
            OnFireAction = null;

            // Add this element to the DiagnosticMessagesService, if possible
            if (ServiceBroker.CanProvide<DiagnosticMessagesService>())
                ServiceBroker.GetService<DiagnosticMessagesService>().Add(this);

            if (FiringCondition != null)
                FiringCondition.ValueChanged += FiringCondition_ValueChanged;
        }

        private void FiringCondition_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            if (e.NewValueAsBool)
                Fire();
        }

        #endregion Private methods

        #region Event handler

        /// <summary>
        /// The <see cref="ValueChanged"/> event handler
        /// for the <see cref="Value"/> property
        /// </summary>
        public event EventHandler<FiredEventArgs> Fired
        {
            add
            {
                lock (eventLock)
                    FiredHandler += value;
            }
            remove
            {
                lock (eventLock)
                    FiredHandler -= value;
            }
        }

        /// <summary>
        /// The value changed event handler
        /// </summary>
        private EventHandler<FiredEventArgs> FiredHandler;

        /// <summary>
        /// On value changed event
        /// </summary>
        /// <param name="e">The <see cref="ValueChangedEventArgs"/></param>
        protected virtual void OnMessageFired(FiredEventArgs e)
        {
            FiredHandler?.Invoke(this, e);
        }

        #endregion Event handler
    }
}