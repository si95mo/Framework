using Core;
using Core.Conditions;
using Devices;
using Hardware;

namespace Diagnostic.Messages
{
    /// <summary>
    /// Implement the <see cref="IDiagnosticMessage"/> interface as an alarm. <br/>
    /// If <see cref="DiagnosticMessage.Source"/> is not <see langword="null"/>, in the fire event 
    /// the relative <c>Stop</c> method will be called in case of a <see cref="IResource"/> or <see cref="IDevice"/>
    /// </summary>
    public class Alarm : DiagnosticMessage
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <remarks>The instance will automatically be added to the <see cref="DiagnosticMessagesService"/>, if possible</remarks>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="sourceCode">The source code</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public Alarm(string code, string message, string sourceCode = "", ICondition firingCondition = null) 
            : base(code, message, sourceCode, firingCondition)
        { }

        /// <summary>
        /// Create a new instance of <see cref="Alarm"/>
        /// </summary>
        /// <remarks>The instance will automatically be added to the <see cref="DiagnosticMessagesService"/>, if possible</remarks>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the <see cref="Alarm"/> to fire</param>
        public Alarm(string code, string message, IProperty source = null, ICondition firingCondition = null) 
            : base(code, message, source, firingCondition)
        { }

        #endregion Constructors

        #region IDiagnosticMessage implementation

        public override void Fire()
        {
            // Stop the source if possible
            if (Source != null)
            {
                if (Source is IResource resource)
                {
                    if (resource.Status.Value == ResourceStatus.Executing) // Stop only if the resource is in executing state
                    {
                        resource.Stop();
                    }
                }
                else if (Source is IDevice device)
                {
                    device.Stop();
                }
            }

            base.Fire();
        }

        #endregion IDiagnosticMessage implementation

        #region Factory methods

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <param name="sourceCode">The source code</param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Alarm New(string code, string message, ICondition firingCondition = null, string sourceCode = "")
            => new Alarm(code, message, sourceCode, firingCondition);

        /// <summary>
        /// Create a new <see cref="Alarm"/>
        /// </summary>
        /// <param name="code">The code</param>
        /// <param name="message">The message</param>
        /// <param name="firingCondition">The <see cref="ICondition"/> that will cause the </param>
        /// <param name="source">The source <see cref="IProperty"/></param>
        /// <returns>The created new instance of <see cref="Alarm"/></returns>
        public static Alarm New(string code, string message, ICondition firingCondition = null, IProperty source = null)
            => new Alarm(code, message, source, firingCondition);

        #endregion Factory methods
    }
}