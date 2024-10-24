using Application.UserIdentity.Commands.ExternalLogin;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Application.UserIdentity.Queries.ExternalLogin
{
    public record ExternalLoginCommand : IRequest<AuthenticationProperties>
    {
        [DefaultValue("Google")]
        public string AuthenticationScheme;

        
        [Required]        
        public string RedirectUrl;
    }

    public class ExternalLoginCommandHandler : IRequestHandler<ExternalLoginCommand, AuthenticationProperties>
    {
        private readonly SignInManager<User> _signInManager;


        public ExternalLoginCommandHandler(IServiceProvider serviceProvider)
        {
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        }

        public async Task<AuthenticationProperties> Handle(ExternalLoginCommand request, CancellationToken cancellationToken)
        {
            AuthenticationProperties properties =  _signInManager.ConfigureExternalAuthenticationProperties(request.AuthenticationScheme, request.RedirectUrl);
            return properties;
        }
    }
}
