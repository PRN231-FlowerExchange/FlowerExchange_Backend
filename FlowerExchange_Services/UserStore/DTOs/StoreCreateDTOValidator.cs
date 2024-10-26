using Domain.Repository;
using FluentValidation;

namespace Application.UserStore.DTOs
{
    public class StoreCreateDTOValidator : AbstractValidator<StoreCreateDTO>
    {
        private readonly IStoreRepository _storepository;
        public StoreCreateDTOValidator(IStoreRepository storeRepository)
        {
            _storepository = storeRepository;

            RuleFor(u => u.Name)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' is required.")
                .MaximumLength(300)
                    .WithMessage("'{PropertyName}' cannot be longer than {MaxLength} characters.")
                .MustAsync(BeUniqueStoreName)
                    .WithMessage("'{PropertyName}' must be unique.'{PropertyValue}' has already existed.")
                    .WithErrorCode("Unique");

            RuleFor(u => u.Slug)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' is required.")
                .MaximumLength(200)
                    .WithMessage("'{PropertyName}' cannot be longer than {MaxLength} characters.")
                .MustAsync(BeUniqeSlug)
                    .WithMessage("'{PropertyName}' must be unique.'{PropertyValue}' has already existed.")
                    .WithErrorCode("Unique");

            RuleFor(u => u.Descriptions)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' is required.");

            RuleFor(u => u.Phones)
                .NotEmpty()
                    .WithMessage("'{PropertyName}' is required.");

        }

        public async Task<bool> BeUniqueStoreName(string name, CancellationToken cancellationToken)
        {
            var store = await _storepository.FirstOrDefaultAsync(u => u.Name.ToLower().Equals(name.Trim().ToLower()));
            if (store != null)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> BeUniqeSlug(string slug, CancellationToken cancellationToken)
        {
            var store = await _storepository.FirstOrDefaultAsync(u => u.Slug.ToLower().Equals(slug.Trim().ToLower()));
            if (store != null)
            {
                return false;
            }
            return true;
        }
    }
}
