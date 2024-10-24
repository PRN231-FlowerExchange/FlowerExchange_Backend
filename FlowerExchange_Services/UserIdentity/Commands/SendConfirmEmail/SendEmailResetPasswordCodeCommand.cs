using Domain.Commons.BaseRepositories;
using Domain.EmailProvider;
using Domain.Entities;
using Domain.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Application.UserIdentity.Commands.SendConfirmEmail
{
    public record class SendEmailResetPasswordCodeCommand : IRequest
    {
        [EmailAddress]
        [Required]
        public string Email { get; init; }

    }

    public class SendEmailResetPasswordCodeHandler : IRequestHandler<SendEmailResetPasswordCodeCommand>
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<SendEmailResetPasswordCodeHandler> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;
        private readonly IEmailProvider _emailProvider;


        public SendEmailResetPasswordCodeHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<SendEmailResetPasswordCodeHandler>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
            _emailProvider = serviceProvider.GetRequiredService<IEmailProvider>();
        }

        public async Task Handle(SendEmailResetPasswordCodeCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(request.Email, nameof(User));
            }
            var codeResetPassword = await _userManager.GeneratePasswordResetTokenAsync(user);
            KeyValuePair<Guid, string> userEmailResetPasswordCode = new KeyValuePair<Guid, string>(user.Id, codeResetPassword);

            string message = $"<h1>Reset password code: <h1></br><h2>{codeResetPassword}</h2>";
            try
            {
                await _emailProvider.SendEmailAsync(request.Email, "Reset Password Code", message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending password reset email to {Email}", request.Email);
                throw;
            }
        }
    }
}
