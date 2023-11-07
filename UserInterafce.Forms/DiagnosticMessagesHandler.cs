using Core.DataStructures;
using Diagnostic;
using Diagnostic.Messages;
using System;
using System.Linq;

namespace UserInterface.Forms
{
    public static class DiagnosticMessagesHandler
    {
        private static DiagnosticMessagesService service;

        public static void Initialize()
        {
            if (ServiceBroker.CanProvide<DiagnosticMessagesService>())
            {
                service = ServiceBroker.GetService<DiagnosticMessagesService>();
            }
            else
            {
                service = new DiagnosticMessagesService();
                Logger.Warn($"{nameof(DiagnosticMessagesService)} not provided by the {nameof(ServiceBroker)}. {nameof(DiagnosticMessagesHandler)} will use its own");
            }

            foreach (IDiagnosticMessage message in service.GetAll().Cast<IDiagnosticMessage>())
            {
                message.Fired += Message_Fired;
            }
        }

        private static void Message_Fired(object sender, FiredEventArgs e)
        {
            DateTime timestamp = e.Timestamp;
            string message = e.Message;
            string longText = e.LongText;
            IDiagnosticMessage diagnosticMessage = e.DiagnosticMessage;

            // Show a new notification here
        }
    }
}