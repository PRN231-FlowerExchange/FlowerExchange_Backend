using Application.Category.DTOs;
using AutoMapper;
using Domain.Commons.BaseRepositories;
using Domain.Entities;
using Domain.Repository;
using MediatR;
using Persistence;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Category.Queries.GetAllCategories
{
    public record GetAllCategoriesQuery : IRequest<List<CategoryDTO>>;

    public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDTO>>
    {
        private readonly ICategoriesRepository _categoryRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesQueryHandler(ICategoriesRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        public async Task<List<CategoryDTO>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<List<CategoryDTO>>(categories);
        }
    }
}
