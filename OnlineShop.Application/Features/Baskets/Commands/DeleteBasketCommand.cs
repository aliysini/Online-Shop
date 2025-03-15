using FluentValidation;
using MediatR;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Baskets.Commands
{
    public class DeleteBasketCommand:IRequest<Unit>
    {
        public string Username  { get; set; }
        public class DeleteBasketCommandHandler : IRequestHandler<DeleteBasketCommand, Unit>
        {
            public DeleteBasketCommandHandler(IBasketRepository basketRepository, IUserRepository userRepository)
            {
                _basketRepository = basketRepository;
                _userRepository = userRepository;
            }
            private readonly IUserRepository _userRepository;
            private readonly IBasketRepository _basketRepository;
            public async Task<Unit> Handle(DeleteBasketCommand request, CancellationToken cancellationToken)
            {
                var validationResult = new DeletBasketCommandValidator().Validate(request);
                if (!validationResult.IsValid)
                {
                    var nessages = validationResult.Errors.Select(erorr => erorr.ErrorMessage);
                    throw new Common.Exeptions.ValidationExeption(nessages);
                }
                var user = _userRepository.GetByUserNameAsync(request.Username);
                if (user == null)
                {
                    throw new Exception($"کاربر با نام {request.Username} پیدا نشد ");
                }
                var basket = await _basketRepository.GetBasketAsync(request.Username);
                if (basket == null)
                {
                    throw new Exception("سبد خریدی برای کاربر پیدا نشد ");
                }
                await _basketRepository.DeleteBasketAsync(request.Username);
                return Unit.Value;
            }
        }
        public class DeletBasketCommandValidator : AbstractValidator<DeleteBasketCommand>
        {
            public DeletBasketCommandValidator()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("نام کاربری نمیتواند خالی باشد");
            }
        }
    }
}
