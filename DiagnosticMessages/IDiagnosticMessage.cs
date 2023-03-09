using Core;
using System;

namespace DiagnosticMessages
{
    /// <summary>
    /// Define a basic diagnostic message interface
    /// </summary>
    public interface IDiagnosticMessage : IProperty
    {
        /// <summary>
        /// The code of the source that caused the <see cref="IDiagnosticMessage"/> to <see cref="Fire"/>
        /// </summary>
        string SourceCode { get; }

        /// <summary>
        /// The <see cref="IDiagnosticMessage"/> message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The <see cref="IDiagnosticMessage"/> firing time (as <see cref="DateTime"/>
        /// </summary>
        DateTime FiringTime { get; }

        /// <summary>
        /// Fire the <see cref="IDiagnosticMessage"/> and stop the relative source idenditified by <see cref="SourceCode"/> (if possible)
        /// </summary>
        void Fire();

        /// <summary>
        /// Reset the <see cref="IDiagnosticMessage"/>
        /// </summary>
        void Reset();
    }
}