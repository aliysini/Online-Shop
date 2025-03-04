using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static OnlineShop.Application.Features.Product.Commands.CreateProductCommand;

namespace OnlineShop.Application.Features.Product.Queries
{
    public class GetProductByNameQuery : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public class GetProductByNameHandler : IRequestHandler<GetProductByNameQuery, ProductDto>
        {
            public GetProductByNameHandler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
            {
                _mapper = mapper;
                _productRepository = productRepository;
                _categoryRepository = categoryRepository;
            }
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            private readonly ICategoryRepository _categoryRepository;
            public async Task<ProductDto> Handle(GetProductByNameQuery request, CancellationToken cancellationToken)
            {
                var validationResult = await new GetProductByNameValidator().ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var massages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(massages);
                }
                var productFromDb = await _productRepository.GetByProductNameAsync(request.Name);
                if (productFromDb == null) 
                {
                    throw new Exception($"محصولی با نام {request.Name} پیدا نشد");
                }
                var productDto = _mapper.Map<ProductDto>(productFromDb);
                return productDto;

            }

        }
        public class GetProductByNameValidator : AbstractValidator<GetProductByNameQuery>
        {
            public GetProductByNameValidator()
            {
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام کالا الزامی است").
                    Length(3, 50).WithMessage("نام کالا باید بین 3 تا 50 حرف باشد");
            }
        }
    }
}
