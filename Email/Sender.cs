using Diagnostic;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Email
{
    public static class Sender
    {
        public static bool Initialized { get; private set; } = false;

        private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);
        private static MimeMessage message;

        public static void Initialize(string nameFrom, string emailFrom)
        {
            message = new MimeMessage();
            message.From.Add(new MailboxAddress(nameFrom, emailFrom));

            Initialized = true;
        }

        public static async Task<bool> SendMessageAsync(string nameTo, string emailTo, string subject, string bodyType, string bodyText)
        {
            bool result = false;

            try
            {
                message.Subject = subject;
                message.Body = new TextPart(bodyType)
                { 
                    Text = bodyText 
                };

                result = await SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                await Logger.ErrorAsync(ex);
            }

            return result;
        }

        internal static async Task<bool> SendMessageAsync(MimeMessage message)
        {
            bool result = false;
            using (SmtpClient client = new SmtpClient())
            {
                try
                {
                    await semaphore.WaitAsync();

                    await client.ConnectAsync("smtp.gmail.com", 465, true, default);
                    await client.AuthenticateAsync("emailfromcsharp@gmail.com", "pwdProva@csharp", default);
                    await client.SendAsync(message, default, null);
                    await client.DisconnectAsync(true, default);
                }
                catch(Exception ex)
                {
                    await Logger.ErrorAsync(ex);
                }
                finally
                {
                    semaphore.Release();
                }
            }

            return result;
        }
    }
}
