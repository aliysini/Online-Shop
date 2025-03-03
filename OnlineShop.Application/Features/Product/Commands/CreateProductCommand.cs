using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Product.Commands
{
    public class CreateProductCommand : IRequest<ProductDto>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get;private set; }
        public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
        {
            public CreateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
                _categoryRepository = categoryRepository?? throw new ArgumentNullException(nameof(categoryRepository));
            }
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;
            private readonly IProductRepository _productRepository;
            public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
            {
                var validationResult = await new CraeteProductCommandValidator().ValidateAsync(command);
                if (!validationResult.IsValid) 
                {
                    var massages = validationResult.Errors.Select(e=> e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(massages);
                }
                var productFromDB = await _productRepository.GetByProductNameAsync(command.Name);
                if (productFromDB != null)
                {
                    throw new Exception($"محصول با نام {command.Name}.قبلا ایجاد شده است ");
                }
                var category =await _categoryRepository.GetByCategoryNameAsync(command.CategoryName);
                if (category == null)
                {
                    throw new Exception($"دسته بندی با{command.CategoryName} نام پیدا نشد ");
                }
                command.CategoryId = category.Id;
                var product = _mapper.Map<Domain.Entity.Product>(command);
                await _productRepository.AddAsync(product);
                var productDto = _mapper.Map<ProductDto>(product);
                productDto.Category = command.CategoryName;
                return productDto;

            }
        }
        public class CraeteProductCommandValidator : AbstractValidator<CreateProductCommand>
        {
            public CraeteProductCommandValidator()
            {
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام کالا الزامی است").
                     Length(3, 50).WithMessage("نام کالا باید بین 3 تا 50 حرف باشد");
                RuleFor(current => current.Price).NotEmpty().WithMessage("وارد کردن قیمت کالا الزامی است")
                    .GreaterThan(0).WithMessage("قیمت باید عددی بالاتر از صفر باشد");
                RuleFor(current => current.CategoryName)
                    .NotEmpty().WithMessage("دسته بندی کالا نمیتواند خالی باشد");
                RuleFor(user => user.Stock)
                    .NotEmpty().WithMessage("وارد کردن تعداد کالا الزامی است")
                    .GreaterThan(0).WithMessage("تعداد باید عددی بالاتر از صفر باشد");
            }
        }
    }
}
