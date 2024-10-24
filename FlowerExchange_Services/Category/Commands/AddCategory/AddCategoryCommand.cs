using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;
using Persistence.RepositoryAdapter;

namespace Application.Category.Commands.AddCategory
{
    public record AddCategoryCommand : IRequest<Guid>
    {
        public string Name { get; init; } = default!;
    }

    public class AddCategoryCommandHandler : IRequestHandler<AddCategoryCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public AddCategoryCommandHandler(IMapper mapper, ICategoriesRepository categoryRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {

            // Kiểm tra xem danh mục đã tồn tại chưa
            if (await _categoryRepository.ExistsByNameAsync(request.Name))
            {
                throw new DuplicateException("Category with the same name already exists."); 
            }
            var category = new Domain.Entities.Category
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Status = CategoryStatus.Active,
            };

            await _categoryRepository.InsertAsync(category);
            await _unitOfWork.SaveChangesAsync();
            return category.Id;
        }
    }
}
