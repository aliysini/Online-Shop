using AutoMapper;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Product.Queries
{
    public class GetAllProductQuery :IRequest<List<ProductDto>>
    {
        public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, List<ProductDto>>
        {
            public GetAllProductQueryHandler(IProductRepository productRepository,IMapper mapper)
            {
                _mapper = mapper;
                _productRepository = productRepository;
            }
            private readonly IMapper _mapper;
            private readonly IProductRepository _productRepository;
            public async Task<List<ProductDto>> Handle(GetAllProductQuery request, CancellationToken cancellationToken)
            {
                var productsFromDB = await _productRepository.GetAllAsync();
                var productDto = _mapper.Map<List<ProductDto>>(productsFromDB);
                return productDto;
            }
        }
    }
}
