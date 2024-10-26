using FluentValidation;

namespace Application.PostFlower.Commands.AddServiceToPostCommand
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
