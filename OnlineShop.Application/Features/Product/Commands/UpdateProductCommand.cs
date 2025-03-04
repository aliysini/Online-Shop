using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Product.Commands
{
    public class UpdateProductCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int Stock { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; private set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Unit>
        {
            public UpdateProductCommandHandler(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
                _productRepository = productRepository;
            }
            private readonly ICategoryRepository _categoryRepository;
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
            {
                  var validationResult = await new UpdateProductCommandValidator().ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var messages = validationResult.Errors.Select(e=>e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(messages);

                }
                var productFromDB = await _productRepository.GetByProductNameAsync(request.Name);
                if (productFromDB == null)
                {
                    throw new Exception($"محصول با نام {request.Name} پیدا نشد");
                }
                if (request.CategoryName == null || request.CategoryName == string.Empty)
                {
                    var category = await _categoryRepository.GetByIdAsync(productFromDB.CategoryId);
                    request.CategoryName = category.Name;
                }
                var categoryFromDb = await _categoryRepository.GetByCategoryNameAsync(request.CategoryName);
                if (categoryFromDb == null)
                {
                    throw new Exception($"دسته بندی با نام {request.CategoryName} پیدا نشد");
                }
                request.CategoryId = categoryFromDb.Id;
                _mapper.Map(request, productFromDB);
                await _productRepository.UpdateAsync(productFromDB);
                return Unit.Value;
            }
        }
        public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
        {
            public UpdateProductCommandValidator()
            {
                RuleFor(current => current.Description).NotEmpty();
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام کالا الزامی است").
                     Length(3, 50).WithMessage("نام کالا باید بین 3 تا 50 حرف باشد");
                RuleFor(current => current.Price).NotEmpty().WithMessage("وارد کردن قیمت کالا الزامی است")
                    .GreaterThan(0).WithMessage("قیمت باید عددی بالاتر از صفر باشد");
                RuleFor(user => user.Stock)
                    .NotEmpty().WithMessage("وارد کردن تعداد کالا الزامی است")
                    .GreaterThan(0).WithMessage("تعداد باید عددی بالاتر از صفر باشد");
            }
        }

    }
}
