using Domain.Notification;
using Infrastructure.Notification.Email.Fake;
using Infrastructure.Notification.Email.SendGrid;
using Infrastructure.Notification.Email.SmtpClient;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification.Email
{
    public static class EmailNotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddSmtpClientEmailNotification(this IServiceCollection services, SmtpClientOptions options)
        {
            services.AddSingleton<IEmailNotification>(new SmtpClientEmailNotification(options));
            return services;
        }

        public static IServiceCollection AddFakeEmailNotification(this IServiceCollection services)
        {
            services.AddSingleton<IEmailNotification>(new FakeEmailNotification());
            return services;
        }

        public static IServiceCollection AddSendGridEmailNotification(this IServiceCollection services, SendGridOptions options)
        {
            services.AddSingleton<IEmailNotification>(new SendGridEmailNotification(options));
            return services;
        }

        public static IServiceCollection AddEmailNotification(this IServiceCollection services, EmailOptions options)
        {
            if (options.UsedFake())
            {
                services.AddFakeEmailNotification();
            }
            else if (options.UsedSmtpClient())
            {
                services.AddSmtpClientEmailNotification(options.SmtpClient);
            }
            else if (options.UsedSendGrid())
            {
                services.AddSendGridEmailNotification(options.SendGrid);
            }

            return services;
        }
    }
}
