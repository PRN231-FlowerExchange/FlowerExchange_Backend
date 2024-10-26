using Application.UserStore.DTOs;
using Domain.Repository;
using FluentValidation;

namespace Application.UserStore.Command.CreateUserStore
{
    public class CreateUserStoreCommandValidator : AbstractValidator<CreateUserStoreCommand>
    {
        public CreateUserStoreCommandValidator(IStoreRepository storeRepository)
        {
            RuleFor(u => u.StoreCreateDTO)
                .SetValidator(new StoreCreateDTOValidator(storeRepository));
        }
    }
}
