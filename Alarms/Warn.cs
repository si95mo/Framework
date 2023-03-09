using Core;
using Core.Conditions;
using Devices;
using Hardware;
using System;

namespace Alarms
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
        public Warn(string code, string sourceCode, string message, ICondition firingCondition) : base(code, sourceCode, message, firingCondition)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Warn"/>
        /// </summary>
        /// <remarks>The instance will automatically be added to the <see cref="DiagnosticMessagesService"/>, if possible</remarks>
        /// <param name="code"></param>
        /// <param name="source"></param>
        /// <param name="message"></param>
        /// <param name="firingCondition"></param>
        public Warn(string code, IProperty source, string message, ICondition firingCondition) : base(code, source, message, firingCondition)
        { }

        #region IDiagnosticMessage implementation

        public override void Fire()
        {
            Fired.Value = true;
            FiringTime = DateTime.Now;

            OnFireAction?.Invoke();

            // Stop the source if possible
            if (Source is IResource)
                (Source as IResource).Stop();
            else if (Source is IDevice)
                (Source as IDevice).Stop();
        }

        #endregion IDiagnosticMessage implementation

        #region Factory methods

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Warn New(string code, string sourceCode, string message, ICondition firingCondition)
            => new Warn(code, sourceCode, message, firingCondition);

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Warn New(string code, IProperty source, string message, ICondition firingCondition)
            => new Warn(code, source, message, firingCondition);

        #endregion Factory methods
    }
}
