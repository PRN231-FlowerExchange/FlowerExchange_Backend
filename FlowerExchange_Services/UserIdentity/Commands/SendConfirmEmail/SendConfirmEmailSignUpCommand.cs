using Application.Services.EmailForIdentityService;
using Domain.EmailProvider;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;

namespace Application.UserIdentity.Commands.SendConfirmEmail
{
    public record class SendConfirmEmailSignUpCommand : IRequest<string>
    {
        [EmailAddress]
        [Required]
        public string Email { get; init; }
    }

    public class SendConfirmEmailSignUpCommandHandler : IRequestHandler<SendConfirmEmailSignUpCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SendConfirmEmailSignUpCommand> _logger;
        private readonly EmailForIdentityService _emailForIdentityService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailProvider _emailProvider;

        public SendConfirmEmailSignUpCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<SendConfirmEmailSignUpCommand>>();
            _emailForIdentityService = serviceProvider.GetRequiredService<EmailForIdentityService>();
            _urlHelperFactory = serviceProvider.GetRequiredService<IUrlHelperFactory>();
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _emailProvider = serviceProvider.GetRequiredService<IEmailProvider>();
        }

        public async Task<string> Handle(SendConfirmEmailSignUpCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.Email);
            }

            if (user.EmailConfirmed)
            {
                throw new ConflictException("Your email has been confirmed already. No further emails will be sent.");
            }

            Guid userId = Guid.Parse(await _userManager.GetUserIdAsync(user));
            var emailConfirmationCodeBase64UrlEncode = await _emailForIdentityService.GenerateEmailConfirmationBase64Code(user);
            KeyValuePair<Guid, string> userEmailConfirmationCode = new KeyValuePair<Guid, string>(userId, emailConfirmationCodeBase64UrlEncode);

            var actionContext = new ActionContext(_httpContextAccessor.HttpContext, _httpContextAccessor.HttpContext.GetRouteData(), new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor());
            var urlHelper = _urlHelperFactory.GetUrlHelper(actionContext);

            var stringUrlCallback = urlHelper.Action(
                action: "ConfirmEmailWhenRegister",
                controller: "Auth",
                values: new { UserId = userEmailConfirmationCode.Key, Code = userEmailConfirmationCode.Value },
                protocol: _httpContextAccessor.HttpContext.Request.Scheme);
            if (stringUrlCallback == null)
            {
                throw new ArgumentNullException(nameof(stringUrlCallback));
            }

            string message = $"<p>Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(stringUrlCallback)}'>link here</a><p>";

            await _emailProvider.SendEmailAsync(request.Email, "Confirm your email", message);

            return "Send email successfully !";
        }
    }
}
