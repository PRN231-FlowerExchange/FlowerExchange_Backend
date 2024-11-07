using FluentValidation;

namespace Application.Wallet.Commands.CreateWalletWithdrawTransaction;

public class CreateWalletWithdrawTransactionCommandValidator : AbstractValidator<CreateWalletWithdrawTransactionCommand>
{
    public CreateWalletWithdrawTransactionCommandValidator()
    {
        RuleFor(w => w.Amount)
            .NotNull()
            .GreaterThanOrEqualTo(50000)
            .WithMessage("Amount must be greater than 50000!");
        
        // RuleFor(w => w.UserId)
        //     .NotNull()
        //     .NotEmpty()
        //     .WithMessage("UserId is required!");
    }
}