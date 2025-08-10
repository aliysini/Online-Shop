using FluentValidation;
using MediatR;
using OnlineShop.Domain.Contracts;

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
                var validationResult = await new DeleteProductCommandValidator().ValidateAsync(request);
                if (!validationResult.IsValid)
                {
                    var messages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(messages);
                }
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
