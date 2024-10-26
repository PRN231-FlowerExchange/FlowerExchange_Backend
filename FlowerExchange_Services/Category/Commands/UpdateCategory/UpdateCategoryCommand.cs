using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;

namespace Application.Category.Commands.UpdateCategory
{
    public record UpdateCategoryCommand : IRequest<Guid>
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
        public CategoryStatus Status { get; init; } = default!;
    }

    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Guid>
    {
        private readonly IMapper _mapper;
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public UpdateCategoryCommandHandler(IMapper mapper, ICategoriesRepository categoryRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) throw new NotFoundException("Category not found");

            // Cập nhật thông tin danh mục
            category.Name = request.Name;
            category.Status = request.Status;

            // Cập nhật trong repository
            await _categoryRepository.UpdateAsync(category);
            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }
    }
}
