using AutoMapper;
using OnlineShop.Application.Commands.User;
using OnlineShop.Application.Dtos.User;
using OnlineShop.Application.Interfaces;
using OnlineShop.Application.Queries.User;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Services
{
    public class UserService
    {
        public UserService(IWriteRepository<User> writeRepository, IReadRepository<User> readRepository, IMapper mapper)
        {
            _userWriteRepository = writeRepository;
            _userReadRepository = readRepository;
            _mapper = mapper;
        }
        private readonly IWriteRepository<User> _userWriteRepository;
        private readonly IReadRepository<User> _userReadRepository;
        private readonly IMapper _mapper;
        public async Task<IEnumerable<UserDto>> GetAllUserAsync()
        {
            var usersFromDataBase = await _userReadRepository.GetAllAsync();
            var usersDto = _mapper.Map<IEnumerable<UserDto>>(usersFromDataBase);
            return usersDto;
        }
        public async Task<UserDto> GetUserAsync(GetUserById getUserById)
        {
            var userFromDataBase = await _userReadRepository.GetByIdAsync(getUserById.Id);
            var userDto = _mapper.Map<UserDto>(userFromDataBase);
            return userDto;
        }

        public async Task<UserDto> CreateUserAsync(CreateUserCommand createUserCommand)
        {
            var user = new User()
            {
                FullName = createUserCommand.UserName,
                Email = createUserCommand.Email,
                Address = createUserCommand.Address,
                CreatedDate = DateTime.Now,
                Password = createUserCommand.Password,
                UserName = createUserCommand.UserName,
            };
            await _userWriteRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }
        public async Task<UserDto> UpdateUserAsync(UpdateUserCommand Command)
        {
            var user = await _userReadRepository.GetByIdAsync(Command.Id);
            if (user == null)
            {
                throw new Exception("not found user!");
            }
            user.UserName = Command.UserName;
            user.Email = Command.Email;
            user.FullName = Command.FullName;
            user.Address = Command.Address;
            user.Password = Command.Password;
            await _userWriteRepository.UpdateAsync(user);
            return _mapper.Map<UserDto>(user);
        }
        public async Task DeleteUserAsynnc(DeleteUserCommand deleteUserCommand)
        {
            var user = await _userReadRepository.GetByIdAsync(deleteUserCommand.Id);
            if (user == null)
            {
                throw new Exception("not found user!");
            }
            await _userWriteRepository.DeleteAsync(user);

        }

    }
}
