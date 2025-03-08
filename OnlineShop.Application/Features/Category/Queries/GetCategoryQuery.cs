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
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public string Name;
        public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, CategoryDto>
        {
            public GetCategoryQueryHandler(ICategoryRepository categoryRepository,IMapper mapper)
            {
                _mapper = mapper;
                _categoryRepository = categoryRepository;
            }
            private readonly IMapper _mapper;
            private readonly ICategoryRepository _categoryRepository;
            public async Task<CategoryDto> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
            {
                var category = await _categoryRepository.GetByCategoryNameAsync(request.Name);
                if (category == null)
                {
                    throw new Exception($"دسته بندی با نام {request.Name} پیدا نشد");
                }
                return _mapper.Map<CategoryDto>(category);
            }
        }
    }
}
