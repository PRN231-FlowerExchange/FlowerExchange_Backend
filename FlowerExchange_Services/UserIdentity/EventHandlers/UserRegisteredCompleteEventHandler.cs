using Domain.Entities;
using Domain.Events.UserEvents;
using Domain.Services.UserWallet;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;


namespace Application.UserIdentity.EventHandlers
{
    public class UserRegisteredCompleteEventHandler : INotificationHandler<UserRegisteredCompleteEvent>
    {
        private readonly ILogger<UserRegisteredCompleteEventHandler> _logger;
        private readonly IUserWalletService _userWalletService;

        public UserRegisteredCompleteEventHandler(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<UserRegisteredCompleteEventHandler>>();
            _userWalletService = serviceProvider.GetRequiredService<IUserWalletService>();
        }

        public async Task Handle(UserRegisteredCompleteEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Domain Event: ", notification.GetType().Name);
            try
            {
                _logger.LogInformation("Start created user wallet ");
                Wallet wallet = await _userWalletService.CreateUserWallet(notification.User);
                if (wallet != null)
                {
                    _logger.LogInformation("Success wallet created ");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event: " + ex.Message);
            }
        }


    }
}
