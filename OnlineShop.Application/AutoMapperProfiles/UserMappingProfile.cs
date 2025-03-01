using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Domain.Entity;

namespace OnlineShop.Application.AutoMapperProfiles
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User,UserDto>();
            CreateMap<CreateUserCommand, User>().ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(src => DateTime.UtcNow));
            CreateMap<UpdateUserCommand,User>();
            
        }
    }
}
