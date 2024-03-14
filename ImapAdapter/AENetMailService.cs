using AE.Net.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.ImapAdapter
{
    internal class AENetMailService : IImapConnectService
    {
        ImapClient _client;
        public void Connect(string hostname, int port, bool useSsl, string email, string password)
        {
            _client = new ImapClient(hostname, email, password, AuthMethods.Login, port, true);
        }

        public void Disconnect()
        {
            if (_client != null) return;

            _client.Logout();
            _client.Dispose();
        }

        public void SaveAttachment(string companyEmail)
        {
            
            
            if (_client == null) throw new Exception("_client is null");

            var messages = _client.SearchMessages(
                    SearchCondition.Undeleted().And(
                    SearchCondition.From(companyEmail)));

            if (messages == null) throw new Exception("messages is null");
            if (messages.Count() == 0)
            {
                Console.WriteLine("Сообщений от данной компании нет");
                throw new Exception("messages.Count() == 0");
            }

            var message = messages.Last();
            var attachments = message.Value.Attachments;

            AE.Net.Mail.Attachment attachment = null;

            foreach (var att in attachments)
            {
                if (att.Filename.Contains(".csv"))
                {
                    attachment = att;
                }
            }

            if (attachment == null) throw new Exception("email's attachment does not contain file with .csv format");

            string fileName = "downloadedData.csv";
            string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);
            attachment.Save(fullPath);
            Console.WriteLine("Файл будет сохранен в директории: " + fullPath);
        }
    }
}
