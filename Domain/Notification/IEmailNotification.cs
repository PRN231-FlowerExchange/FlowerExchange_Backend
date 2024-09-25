using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Notification
{
    public interface IEmailNotification
    {
        Task SendAsync(IEmailMessage emailMessage, CancellationToken cancellationToken = default);
    }

    public interface IEmailMessage
    {
        public string From { get; set; }

        public string Tos { get; set; }

        public string CCs { get; set; }

        public string BCCs { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }
    }
}
