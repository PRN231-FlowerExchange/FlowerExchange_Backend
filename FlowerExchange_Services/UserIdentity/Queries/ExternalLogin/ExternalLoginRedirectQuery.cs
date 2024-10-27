using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Application.UserIdentity.Queries.ExternalLogin
{
    public record ExternalLoginRedirectQuery : IRequest<AuthenticationProperties>
    {
        [DefaultValue("Google")]
        public string AuthenticationScheme;


        [Required]
        public string RedirectUrl;
    }

    public class ExternalLoginRedirectQueryHandler : IRequestHandler<ExternalLoginRedirectQuery, AuthenticationProperties>
    {
        private readonly SignInManager<User> _signInManager;


        public ExternalLoginRedirectQueryHandler(IServiceProvider serviceProvider)
        {
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        }

        public async Task<AuthenticationProperties> Handle(ExternalLoginRedirectQuery request, CancellationToken cancellationToken)
        {
            AuthenticationProperties properties = _signInManager.ConfigureExternalAuthenticationProperties(request.AuthenticationScheme, request.RedirectUrl);
            return properties;
        }
    }
}
