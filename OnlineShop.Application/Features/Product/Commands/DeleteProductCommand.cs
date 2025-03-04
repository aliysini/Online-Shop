using FluentValidation;
using MediatR;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Product.Commands
{
    public class DeleteProductCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Unit>
        {
            public DeleteProductCommandHandler(IProductRepository productRepository)
            {
                _productRepository = productRepository;
            }
            private readonly IProductRepository _productRepository;
            public async Task<Unit> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
            {
                var productFromDB = await _productRepository.GetByProductNameAsync(request.Name);
                if (productFromDB == null)
                {
                    throw new Exception($"کالا با نام {request.Name} پیدا نشد ");
                }
                await _productRepository.DeleteAsync(productFromDB);
                return Unit.Value;
            }
        }

        public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
        {
            public DeleteProductCommandValidator()
            {
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام کالا الزامی است").
                     Length(3, 50).WithMessage("نام کالا باید بین 3 تا 50 حرف باشد");
            }
        }
    }
}
