using Core.DataStructures;
using DiagnosticMessages;
using System;

namespace UserInterface.Forms
{
    public static class DiagnositcMessagesHandler
    {
        private static DiagnosticMessagesService service;

        public static void Initialize()
        {
            service = ServiceBroker.CanProvide<DiagnosticMessagesService>() ? ServiceBroker.GetService<DiagnosticMessagesService>() : new DiagnosticMessagesService();
            foreach(DiagnosticMessage message in service.GetAll())
                message.Fired += Message_Fired;
        }

        private static void Message_Fired(object sender, FiredEventArgs e)
        {
            DateTime timestamp = e.Timestamp;
            string message = e.Message;

            // Show a new notification here
        }
    }
}
