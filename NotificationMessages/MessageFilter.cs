using Diagnostic;
using System.Windows.Forms;

namespace NotificationMessages
{
    internal enum MessageType
    {
        User = 0x400
    }

    /// <summary>
    /// Implement a message filter between <see cref="Sender"/> and <see cref="Receiver"/>
    /// </summary>
    public class MessageFilter : IMessageFilter
    {
        public bool PreFilterMessage(ref Message m)
        {
            bool result = false;

            if ((MessageType)m.Msg == MessageType.User)
            {
                result = true;
                Logger.Info($"Message received {m.Msg}");
            }

            return result;
        }
    }
}