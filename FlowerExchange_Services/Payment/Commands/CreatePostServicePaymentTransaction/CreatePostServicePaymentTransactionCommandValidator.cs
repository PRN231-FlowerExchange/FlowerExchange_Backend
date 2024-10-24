using Application.Payment.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.Commands.CreatePostServicePaymentTransaction
{
    public class CreatePostServicePaymentTransactionCommandValidator : AbstractValidator<CreatePostServicePaymentTransactionCommand>
    {
        public CreatePostServicePaymentTransactionCommandValidator()
        {
            RuleFor(ps => ps.postServicePaymentRequest)
                .NotNull()
                .WithMessage("postServicePaymentRequest is required!")
                .SetValidator(new PostServicePaymentRequestValidator());

            RuleFor(ps => ps.userId)
                .NotNull()
                .WithMessage("User id is required!");
        }
    }
}
