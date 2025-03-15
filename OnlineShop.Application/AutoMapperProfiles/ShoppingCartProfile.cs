using AutoMapper;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Baskets.Commands;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.AutoMapperProfiles
{
    public class ShoppingCartProfile : Profile
    {
        public ShoppingCartProfile()
        {
            CreateMap<ShoppingCart, ShoppingCartDto>();
            CreateMap<CreateBasketCommand,ShoppingCart>();
        }
    }
}
