using Core;
using Core.Conditions;
using Devices;
using Hardware;
using System;

namespace DiagnosticMessages
{
    /// <summary>
    /// Implement the <see cref="IDiagnosticMessage"/> interface as a warn
    /// </summary>
    public class Warn : DiagnosticMessage
    {
        /// <summary>
        /// Create a new instance of <see cref="Warn"/>
        /// </summary>
        /// <remarks>The instance will automatically be added to the <see cref="DiagnosticMessagesService"/>, if possible</remarks>
        /// <param name="code"></param>
        /// <param name="sourceCode"></param>
        /// <param name="message"></param>
        /// <param name="firingCondition"></param>
        public Warn(string code, string message, string sourceCode = null, ICondition firingCondition = null) 
            : base(code, message, sourceCode, firingCondition)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Warn"/>
        /// </summary>
        /// <remarks>The instance will automatically be added to the <see cref="DiagnosticMessagesService"/>, if possible</remarks>
        /// <param name="code"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="firingCondition"></param>
        public Warn(string code, string message, IProperty source = null, ICondition firingCondition = null) 
            : base(code, message, source, firingCondition)
        { }

        #region IDiagnosticMessage implementation

        public override void Fire()
        {
            Fired.Value = true;
            FiringTime = DateTime.Now;

            OnFireAction?.Invoke();
        }

        #endregion IDiagnosticMessage implementation

        #region Factory methods

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Warn"/> to fire</param>
        /// <returns>The created new instance of <see cref="Warn"/></returns>
        public static Warn New(string code, string message, ICondition firingCondition, string sourceCode = null)
            => new Warn(code, message, sourceCode, firingCondition);

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Warn"/> to fire</param>
        /// <returns>The created new instance of <see cref="Warn"/></returns>
        public static Warn New(string code, string message, ICondition firingCondition, IProperty source = null)
            => new Warn(code, message, source, firingCondition);

        #endregion Factory methods
    }
}