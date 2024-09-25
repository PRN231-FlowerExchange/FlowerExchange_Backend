using Domain.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification.Email.Fake
{
    public class FakeEmailNotification : IEmailNotification
    {
        public Task SendAsync(IEmailMessage emailMessage, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
    }
}
