using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Payment.DTOs
{
    public class PostServicePaymentRequestValidator : AbstractValidator<PostServicePaymentRequest>
    {
        public PostServicePaymentRequestValidator()
        {
            RuleFor(ps => ps.PostIds)
                .NotEmpty()
                .NotNull()
                .WithMessage("Post ids is required!");

            RuleFor(ps => ps.ServiceIds)
                .NotEmpty()
                .NotNull()
                .WithMessage("Service ids is required!");
        }
    }
}
