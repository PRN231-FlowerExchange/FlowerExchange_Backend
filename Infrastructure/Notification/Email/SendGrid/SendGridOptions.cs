using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification.Email.SendGrid
{
    public class SendGridOptions
    {
        public string ApiKey { get; set; }

        public string OverrideFrom { get; set; }

        public string OverrideTos { get; set; }
    }
}
