using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
