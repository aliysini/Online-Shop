using FluentValidation;
using MediatR;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Category.Commands
{
    public class DeleteCategoryCommand : IRequest<Unit>
    {
        public string Name {  get; set; }
        public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Unit>
        {
            public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository,IProductRepository productRepository)
            {
                _categoryRepository = categoryRepository;
                _productRepository = productRepository;
            }
            private readonly IProductRepository _productRepository;
            private readonly ICategoryRepository _categoryRepository;
            public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
            {
                var validationResult = new DeleteCategoryCommandValidator().Validate(request);
                if (!validationResult.IsValid)
                {
                    var messages = validationResult.Errors.Select(e=>e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(messages);
                }
                var category = await _categoryRepository.GetByCategoryNameAsync(request.Name);
                if (category  == null)
                {
                    throw new Exception("دسته بندی پیدا نشد");
                }
                var products =await _productRepository.GetByCategoryIdAsync(category.Id);
                foreach (var product in products)
                {
                    product.CategoryId = null;
                    await _productRepository.UpdateAsync(product);
                }
                await _categoryRepository.IsDeleteAsync(category);
                return Unit.Value;
            }
        }
        public class DeleteCategoryCommandValidator: AbstractValidator<DeleteCategoryCommand>
        {
            public DeleteCategoryCommandValidator()
            {
                RuleFor(current=>current.Name).NotEmpty().WithMessage("وارد کردن نام دسته بندی اجباری است")
                     .Length(3, 50).WithMessage("نام دسته بندی باید بین 3 تا 50 حرف باشد");
            }
        }
    }
}
