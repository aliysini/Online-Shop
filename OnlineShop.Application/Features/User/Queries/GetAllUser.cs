using AutoMapper;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.User.Queries
{
    public class GetAllUser:IRequest<IEnumerable<UserDto>>
    {
        public class GetAllUserHandler:IRequestHandler<GetAllUser,IEnumerable<UserDto>>
        {
            public GetAllUserHandler(IUserRepository userRepository,IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper??throw new ArgumentNullException(nameof(mapper));
            }
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public async Task<IEnumerable<UserDto>> Handle(GetAllUser getAllUser,CancellationToken cancellationToken)
            {
                var usersFromDB = await _userRepository.GetAllAsync();
                var users = _mapper.Map<List<UserDto>>(usersFromDB);
                return users;
                
            }
        }
    }
}
