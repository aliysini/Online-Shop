using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static OnlineShop.Application.Features.User.Commands.DeleteUserCommand;

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
                var validationUser = await new GetUserByIdValidator().ValidateAsync(getUserById);
                if (!validationUser.IsValid)
                {
                    var errorMassage = validationUser.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(errorMassage);
                }
                var userFromDB = await _userRepository.GetByIdAsync(getUserById.Id);
                if (userFromDB == null)
                {
                    throw new Exception("کاربر پیدا نشد");
                }
                var userDto = _mapper.Map<UserDto>(userFromDB);
                return userDto;
            }
        }
        public class GetUserByIdValidator:AbstractValidator<GetUserById>
        {
            public GetUserByIdValidator()
            {
                RuleFor(current => current.Id).NotEmpty().WithMessage("وارد کردن شناسه کاربری اجباری است");
            }
        }
    }
}
