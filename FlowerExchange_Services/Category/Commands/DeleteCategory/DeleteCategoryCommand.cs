using Domain.Commons.BaseRepositories;
using Domain.Constants.Enums;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;
using Persistence;

namespace Application.Category.Commands.DeleteCategory
{
    public record DeleteCategoryCommand : IRequest<Guid>
    {
        public Guid Id { get; init; }
    }

    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Guid>
    {
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IUnitOfWork<FlowerExchangeDbContext> _unitOfWork;

        public DeleteCategoryCommandHandler(ICategoriesRepository categoryRepository, IUnitOfWork<FlowerExchangeDbContext> unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) throw new NotFoundException("Category not found");

            // Đổi trạng thái của danh mục sang Inactive
            category.Status = CategoryStatus.Inactive;

            await _unitOfWork.SaveChangesAsync();

            return category.Id;
        }
    }
}
