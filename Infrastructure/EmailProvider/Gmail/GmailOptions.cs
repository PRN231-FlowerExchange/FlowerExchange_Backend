using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.EmailProvider.Gmail
{

    public enum EmailHostType
    {
        Google,
        Microsoft
    }

    public class GmailOptions
    {
        public EmailHostType HostType { get; set; }
        public string SmtpServer { get; set; }
        public int Port { get; set; }
        public string AccountName { get; set; }
        public string Password { get; set; }
        public bool EnableSSL { get; set; }
        public bool UseCredential { get; set; }
        public string FromEmailAddress { get; set; }
        public string FromDisplayName { get; set; }
    }


}
