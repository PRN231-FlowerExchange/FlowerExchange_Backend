using Application.Common.Models;
using Domain.Commons.BaseRepositories;
using Domain.EmailProvider;
using Domain.Entities;
using Domain.Exceptions;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserIdentity.Commands.ForgotPassword
{

    public record class VerifyResetPasswordCodeCommand : IRequest<bool>
    {
        [EmailAddress]
        [Required]
        public string Email { get; init; }

        [Required]
        public string Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Code { get; init; }

    }

    public class VerifyResetPasswordCodeCommandHandler : IRequestHandler<VerifyResetPasswordCodeCommand, bool>  
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<VerifyResetPasswordCodeCommandHandler> _logger;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitofwork;


       
        public VerifyResetPasswordCodeCommandHandler(IServiceProvider serviceProvider)
        {
            _userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            _logger = serviceProvider.GetRequiredService<ILogger<VerifyResetPasswordCodeCommandHandler>>();
            _unitofwork = serviceProvider.GetRequiredService<IUnitOfWork<FlowerExchangeDbContext>>();
        }
        public async Task<bool> Handle(VerifyResetPasswordCodeCommand request, CancellationToken cancellationToken)
        {
           var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new NotFoundException(request.Email, nameof(User));
            }
            // Validate the password against configured password validators
            var passwordValidationResult =  await ValidatePasswordAsync(user, request.Password);
            if (!passwordValidationResult.Succeeded)
            {
                List<ValidationFailure> validationFailures = passwordValidationResult.Errors
                    .Select(e => new ValidationFailure(e.Code, e.Description))
                    .ToList();
                throw new Domain.Exceptions.ValidationException(validationFailures);
            }

            // Verify the reset password code and reset the password
            var verifyForgotPasswordCode = await _userManager.ResetPasswordAsync(user, request.Code, request.Password);
            var very = await _userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultPhoneProvider, "ResetPassword", request.Code);
            if (!verifyForgotPasswordCode.Succeeded)
            {
                List<ValidationFailure> errors = verifyForgotPasswordCode.Errors
                    .Select(e => new ValidationFailure(e.Code, e.Description))
                    .ToList();
                throw new Domain.Exceptions.ValidationException(errors);
            }
            _unitofwork.SaveChanges();

            return true;
           
        }
        private async Task<IdentityResult> ValidatePasswordAsync(User user, string password)
        {
            var result = IdentityResult.Success;
            foreach (var validator in _userManager.PasswordValidators)
            {
                var validationResult = await validator.ValidateAsync(_userManager, user, password);
                if (!validationResult.Succeeded)
                {
                    result = IdentityResult.Failed(validationResult.Errors.ToArray());
                }
            }
            return result;
        }
    }

}
