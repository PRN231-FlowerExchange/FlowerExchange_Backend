using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Application.Services.EmailForIdentityService
{
    public class EmailForIdentityService
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<User> _logger;



        public EmailForIdentityService(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<User>>();


        }

        public async Task<string> GenerateEmailConfirmationBase64Code(User user)
        {
            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            Console.WriteLine("==== ORIGINAL CODE EMAIL: " + emailConfirmationCode);
            var emailConfirmationCodeBase64UrlEncode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailConfirmationCode));
            return emailConfirmationCodeBase64UrlEncode;
        }


    }

}
