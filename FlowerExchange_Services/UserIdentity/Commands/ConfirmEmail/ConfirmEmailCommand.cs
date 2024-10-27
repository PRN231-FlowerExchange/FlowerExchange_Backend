using Domain.Entities;
using Domain.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ValidationException = Domain.Exceptions.ValidationException;

namespace Application.UserIdentity.Commands.ConfirmEmail
{
    public record class ConfirmEmailCommand : IRequest<string>
    {
        [Required(ErrorMessage = "User id is required !")]
        public string UserId { get; init; }
        [Required(ErrorMessage = "Code is required !")]
        public string Code { get; init; }
    }

    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, string>
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserStore<User> _userStore;
        private readonly ILogger<ConfirmEmailCommand> _logger;


        public ConfirmEmailCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _userStore = serviceProvider.GetRequiredService<IUserStore<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<ConfirmEmailCommand>>();


        }
        public async Task<string> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            Console.WriteLine("USER ID REQUIRE CONFIRM EMAIL: " + request.UserId);
            User user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), request.UserId);
            }
            byte[] codeBytes = WebEncoders.Base64UrlDecode(request.Code);
            string codeAfterDecode = Encoding.UTF8.GetString(codeBytes);

            Console.WriteLine("CODE STRING AFTER DECODE: " + codeAfterDecode);

            IdentityResult result = await _userManager.ConfirmEmailAsync(user, codeAfterDecode);
            if (result.Errors.Any())
            {
                var failures = result.Errors.Select(e => new ValidationFailure(e.Code, e.Description)).ToList();
                throw new ValidationException(failures);
            }
            return "Confirmed Email Successfully";
        }
    }

}
