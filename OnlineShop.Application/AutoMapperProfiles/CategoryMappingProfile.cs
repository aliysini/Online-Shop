using AutoMapper;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Category;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.AutoMapperProfiles
{
    public class CategoryMappingProfile:Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryCommand, Category>();
            CreateMap<Category, CategoryDto>();

        }
    }
}
