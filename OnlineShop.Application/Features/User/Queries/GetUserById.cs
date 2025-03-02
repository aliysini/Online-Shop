using AutoMapper;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.User.Queries
{
    public class GetUserById:IRequest<UserDto>
    {
        public int Id { get; set; }
        
        public class GetUserByIdHandler : IRequestHandler<GetUserById, UserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUserByIdHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            }
            public async Task<UserDto> Handle(GetUserById getUserById,CancellationToken cancellationToken)
            {
                var userFromDB = await _userRepository.GetByIdAsync(getUserById.Id);
                if (userFromDB == null)
                {
                    throw new Exception("کاربر پیدا نشد");
                }
                var userDto = _mapper.Map<UserDto>(userFromDB);
                return userDto;
            }
        }
    }
}
