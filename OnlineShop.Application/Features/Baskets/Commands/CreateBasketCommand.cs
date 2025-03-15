using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Components.Forms.Mapping;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Baskets.Commands
{
    public class CreateBasketCommand :IRequest<ShoppingCartDto>
    {
        public string Username { get; set; }
        public List<CartItemDto> Items { get; set; } = new List<CartItemDto>();
        public class UpdateBasketCommandHandler : IRequestHandler<CreateBasketCommand, ShoppingCartDto>
        {
            public UpdateBasketCommandHandler(IBasketRepository basketRepository,IMapper mapper, 
                IProductRepository productRepository,IUserRepository userRepository)
            {
                _userRepository = userRepository;
                _mapper = mapper;
                _basketRepository = basketRepository;
                _productRepository = productRepository;
            }
            private readonly IUserRepository _userRepository;
            private readonly IProductRepository _productRepository;
            private readonly IMapper _mapper;
            private readonly IBasketRepository _basketRepository;
            public async Task<ShoppingCartDto> Handle(CreateBasketCommand request, CancellationToken cancellationToken)
            { 
                var user =await _userRepository.GetByUserNameAsync(request.Username);
                if (user == null) 
                {
                    throw new Exception("کاربر پیدا نشد");
                }
                var prod = request.Items.ToList();
                foreach (var item in prod)
                {
                    var product = await _productRepository.GetByProductNameAsync(item.ProductName);
                    if (product == null)
                    {
                        throw new Exception("کالا پیدا نشد");
                    }
                    item.Price = product.Price;
                }
              
                var shoppingCart = _mapper.Map<ShoppingCart>(request);
                var basket = await _basketRepository.CreateBasketAsync(shoppingCart);
                var shapingCartDto = _mapper.Map<ShoppingCartDto>(basket);
                return shapingCartDto;
            }
        }
    }
}
