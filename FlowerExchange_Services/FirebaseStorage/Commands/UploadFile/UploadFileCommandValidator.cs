using FluentValidation;

namespace Application.FirebaseStorage.Commands.UploadFile
{
    public class UploadFileCommandValidator : AbstractValidator<UploadFileCommand>
    {
        const long sizeLimit = 5 * 1024 * 1024; // 5 MB

        public UploadFileCommandValidator()
        {
            RuleFor(f => f.File)
                .NotNull()
                .NotEmpty()
                .Must(f => f.Length <= sizeLimit)
                .WithMessage("File size must be less than 5 MB!");
        }
    }
}
