using Infrastructure.Notification.Email.SendGrid;
using Infrastructure.Notification.Email.SmtpClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification.Email
{
    public class EmailOptions
    {
        public string Provider { get; set; }

        public SmtpClientOptions SmtpClient { get; set; }

        public SendGridOptions SendGrid { get; set; }

        public bool UsedFake()
        {
            return Provider == "Fake";
        }

        public bool UsedSmtpClient()
        {
            return Provider == "SmtpClient";
        }

        public bool UsedSendGrid()
        {
            return Provider == "SendGrid";
        }
    }
}
