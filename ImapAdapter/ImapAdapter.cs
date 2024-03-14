using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAndSavePrice.ImapAdapter
{
    internal class ImapAdapter
    {
        IImapConnectService _connectService;
        string _hostname;
        int _port;
        bool _useSsl;
        string _login;
        string _password;

        public ImapAdapter(IImapConnectService connectService, string hostname, int port, bool useSsl, string login, string password)
        {
            _connectService = connectService;
            _hostname = hostname;
            _port = port;
            _useSsl = useSsl;
            _login = login;
            _password = password;
        }

        private void Connect() => _connectService.Connect(_hostname, _port, _useSsl, _login, _password);

        private void Disconnect() => _connectService.Disconnect();

        public void SaveAttachment(string companyEmail)
        {
            Connect();
            _connectService.SaveAttachment(companyEmail);
            Disconnect();
        }
    }
}
