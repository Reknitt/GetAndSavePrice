using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.ImapAdapter
{
    internal interface IImapConnectService
    {
        public void Connect(string hostname, int port, bool useSsl, string login, string password);
        public void Disconnect();
        public void SaveAttachment(string companyEmail);
    }
}
