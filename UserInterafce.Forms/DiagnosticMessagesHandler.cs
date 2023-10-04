using Core.DataStructures;
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
            service = ServiceBroker.CanProvide<DiagnosticMessagesService>() ? ServiceBroker.GetService<DiagnosticMessagesService>() : new DiagnosticMessagesService();
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

            // Show a new notification here
        }
    }
}
