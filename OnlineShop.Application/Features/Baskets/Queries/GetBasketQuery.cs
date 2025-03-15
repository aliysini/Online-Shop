using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using OnlineShop.Persistance.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OnlineShop.Application.Features.Baskets.Commands.DeleteBasketCommand;

namespace OnlineShop.Application.Features.Baskets.Queries
{
    public class GetBasketQuery:IRequest<ShoppingCartDto>
    {
        public string Username {  get; set; }
        public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, ShoppingCartDto>
        {
            public GetBasketQueryHandler(IBasketRepository basketRepository,IMapper mapper )
            {
                _mapper = mapper;
                _basketRepository = basketRepository;
            }
            private readonly IBasketRepository _basketRepository;
            private readonly IMapper _mapper;
            public async Task<ShoppingCartDto> Handle(GetBasketQuery request, CancellationToken cancellationToken)
            {
                var validationResult = new GetBasketQueryValidator().Validate(request);
                if (!validationResult.IsValid)
                {
                    var nessages = validationResult.Errors.Select(erorr => erorr.ErrorMessage);
                    throw new Common.Exeptions.ValidationExeption(nessages);
                }
                var basket = await _basketRepository.GetBasketAsync(request.Username);
                if (basket == null) 
                {
                    return new ShoppingCartDto(username: request.Username);
                }
                var basketDto = _mapper.Map<ShoppingCartDto>(basket);
                return basketDto;
                
            }
        }
        public class GetBasketQueryValidator : AbstractValidator<GetBasketQuery>
        {
            public GetBasketQueryValidator()
            {
                RuleFor(x => x.Username).NotEmpty().WithMessage("نام کاربری نمیتواند خالی باشد");
            }
        }
    }
}
