using FluentValidation;

namespace Application.FirebaseStorage.Commands.DeleteFile
{
    public class DeleteFileCommandValidator : AbstractValidator<DeleteFileCommand>
    {
        public DeleteFileCommandValidator()
        {
            RuleFor(f => f.FileName.Trim())
                .NotEmpty()
                .NotNull()
                .WithMessage("File name is required!");
        }
    }
}
