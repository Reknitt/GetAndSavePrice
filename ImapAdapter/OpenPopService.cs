using OpenPop.Mime;
using OpenPop.Pop3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.ImapAdapter
{
    internal class OpenPopService : IImapConnectService
    {
        Pop3Client _pop3Client;
        public OpenPopService()
        {
            _pop3Client = new Pop3Client();
        }
        public void Connect(string hostname, int port, bool useSsl, string login, string password)
        {
            try
            {
                _pop3Client.Connect(hostname, port, useSsl);
                _pop3Client.Authenticate(login, password);
            } catch (Exception ex)
            {
                Disconnect();
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void Disconnect()
        {
            _pop3Client.Disconnect();
            _pop3Client.Dispose();
        }

        private Message? FindLastEmail(string companyEmail)
        {
            int messageCount = _pop3Client.GetMessageCount();

            for (int i = messageCount; i > 0; i--)
            {
                Message message = _pop3Client.GetMessage(i);
                string from = message.Headers.From.Raw;
                if (from == companyEmail)
                    return message;
            }

            return null;
        }

        public void SaveAttachment(string companyEmail)
        {
            if (_pop3Client.Connected)
            {
                Message? email = FindLastEmail(companyEmail);

                if (email == null)
                    return;

                var attachment = email.FindAllAttachments()[0]; // берем первое вложение
                string fileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, attachment.FileName);
                attachment.Save(new FileInfo(fileName));
                Console.WriteLine("Файл будет сохранен в директории: " + fileName);
            }
        }
    }
}
