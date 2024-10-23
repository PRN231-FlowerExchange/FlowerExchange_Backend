using FluentValidation;
namespace Application.PostFlower.Commands.AddServiceToPostCommand
{
    class AddServiceToPostCommandValidator : AbstractValidator<AddServiceToPostCommand>
    {
        public AddServiceToPostCommandValidator()
        {
            RuleFor(c => c.ListServices)
                .NotEmpty()
                .NotNull()
                .WithMessage("Service required");
            RuleFor(c => c.PostId)
               .NotEmpty()
               .NotNull()
               .WithMessage("Post required");
        }
    }
}
