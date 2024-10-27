using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.UserIdentity.Queries.ExternalLogin
{
    public class ExternalLoginProvidersQuery : IRequest<IList<AuthenticationScheme>>
    {

    }

    public class ExternalLoginProviderQueryHandler : IRequestHandler<ExternalLoginProvidersQuery, IList<AuthenticationScheme>>
    {
        private readonly SignInManager<User> _signInManager;

        public ExternalLoginProviderQueryHandler(IServiceProvider serviceProvider)
        {
            _signInManager = serviceProvider.GetRequiredService<SignInManager<User>>();
        }
        public async Task<IList<AuthenticationScheme>> Handle(ExternalLoginProvidersQuery request, CancellationToken cancellationToken)
        {
            IList<AuthenticationScheme> externalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            return externalLogins;
        }
    }
}
