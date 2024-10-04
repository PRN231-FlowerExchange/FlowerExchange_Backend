using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Post.Commands.AddServiceToPostCommand
{
    class AddServiceToPostCommandValidator : AbstractValidator<AddServiceToPostCommand>
    {
        public AddServiceToPostCommandValidator()
        {
            RuleFor(c => c.ListService)
                .NotEmpty()
                .NotNull()
                .WithMessage("Service required");
            RuleFor(c => c.ServiceDay)
                .NotEmpty()
                .NotNull()
                .GreaterThan(0)
                .WithMessage("Service duration are requied and greater than 0");
        }
    }
}
