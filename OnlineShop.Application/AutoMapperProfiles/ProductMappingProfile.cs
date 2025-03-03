using AutoMapper;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Product.Commands;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.AutoMapperProfiles
{
    public class ProductMappingProfile: Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<CreateProductCommand, Product>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<Product, ProductDto>();
        }

    }
}
