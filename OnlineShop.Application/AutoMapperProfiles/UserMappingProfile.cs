using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using OnlineShop.Application.Dtos.User;
using OnlineShop.Domain.Entity;

namespace OnlineShop.Application.AutoMapperProfiles
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User,UserDto>();
            //CreateMap<UserDto,User>();
        }
    }
}
