using Infrastructure.Notification.Email;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Notification
{
    public static class NotificationServiceCollectionExtensions
    {
        public static IServiceCollection AddNotificationServices(this IServiceCollection services, NotificationOptions options)
        {
            services.AddEmailNotification(options.Email);

            return services;
        }
    }
}
