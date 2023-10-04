﻿using Core;
using System;

namespace Diagnostic.Messages
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
        /// The <see cref="IDiagnosticMessage"/> long text
        /// </summary>
        string LongText { get; }

        /// <summary>
        /// The <see cref="IDiagnosticMessage"/> firing time (as <see cref="DateTime"/>
        /// </summary>
        DateTime FiringTime { get; }

        /// <summary>
        /// Tha active state
        /// </summary>
        bool Active { get; }

        /// <summary>
        /// The fired <see cref="EventHandler"/>
        /// </summary>
        event EventHandler<FiredEventArgs> Fired;

        /// <summary>
        /// Fire the <see cref="IDiagnosticMessage"/> and stop the relative source identified by <see cref="SourceCode"/> (if possible)
        /// </summary>
        void Fire();

        /// <summary>
        /// Fire the <see cref="IDiagnosticMessage"/> with a long text and stop the relative source identified by <see cref="SourceCode"/> (if possible)
        /// </summary>
        void Fire(string longText);

        /// <summary>
        /// Reset the <see cref="IDiagnosticMessage"/>
        /// </summary>
        void Reset();
    }
}