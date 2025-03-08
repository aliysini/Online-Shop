using AutoMapper;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Category.Queries
{
    public class GetAllCategoryQuerirs :IRequest<IEnumerable<CategoryDto>>
    {
        public class GetAllCategoryQuerirsHandler : IRequestHandler<GetAllCategoryQuerirs, IEnumerable<CategoryDto>>
        {
            public GetAllCategoryQuerirsHandler(ICategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;
            public async Task<IEnumerable<CategoryDto>> Handle(GetAllCategoryQuerirs request, CancellationToken cancellationToken)
            {
                var categoryFromDB =await _categoryRepository.GetAllAsync();
                var categoryDto =_mapper.Map<List<CategoryDto>>(categoryFromDB);
                return categoryDto;
            }
        }
    }
}
