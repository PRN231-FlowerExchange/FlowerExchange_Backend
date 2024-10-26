using Application.Category.DTOs;
using AutoMapper;
using Domain.Exceptions;
using Domain.Repository;
using MediatR;

namespace Application.Category.Queries.GetCategory
{
    public record GetCategoryQuery : IRequest<CategoryDTO>
    {
        public Guid Id { get; init; }
    }

    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDTO>
    {
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetCategoryQueryHandler(ICategoriesRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<CategoryDTO> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);
            if (category == null) throw new NotFoundException("Category not found");

            // Chuyển đổi sang DTO
            return _mapper.Map<CategoryDTO>(category);
        }
    }
}
