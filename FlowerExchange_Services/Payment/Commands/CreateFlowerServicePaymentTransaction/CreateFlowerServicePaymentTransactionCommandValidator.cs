using FluentValidation;

namespace Application.Payment.Commands.CreateFlowerServicePaymentTransaction;

public class CreateFlowerServicePaymentTransactionCommandValidator : AbstractValidator<CreateFlowerServicePaymentTransactionCommand>
{
    public CreateFlowerServicePaymentTransactionCommandValidator()
    {
        RuleFor(fs => fs.postId)
            .NotEmpty()
            .NotNull()
            .WithMessage("{PropertyName} is required.");
        
        RuleFor(fs => fs.userId)
            .NotEmpty()
            .NotNull()
            .WithMessage("{PropertyName} is required.");
    }
}