using Core.DataStructures;

namespace DiagnosticMessages
{
    /// <summary>
    /// Define a <see cref="Service{T}"/> for <see cref="IDiagnosticMessage"/>
    /// </summary>
    public class DiagnosticMessagesService : Service<IDiagnosticMessage>
    {
        /// <summary>
        /// Create a new instance of <see cref="DiagnosticMessagesService"/>
        /// </summary>
        public DiagnosticMessagesService() : base()
        { }

        /// <summary>
        /// Create a new instance of <see cref="DiagnosticMessagesService"/>
        /// </summary>
        /// <param name="code">The code</param>
        public DiagnosticMessagesService(string code) : base(code)
        { }
    }
}