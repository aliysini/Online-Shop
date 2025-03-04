using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.User.Commands
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
        {
            public CreateUserCommandHandler(IUserRepository userRepository, IMapper imapper)
            {
                _userRepository= userRepository;
                _mapper = imapper;
            }
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public async Task<UserDto> Handle(CreateUserCommand createUserCommand, CancellationToken cancellationToken)
            {
                var validationResult = await new CreateUserCommandValidator().ValidateAsync(createUserCommand, cancellationToken);
                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(errorMessages);
                }
                var UserFromDatabase = await _userRepository.GetByUserNameAsync(createUserCommand.UserName);
                if (UserFromDatabase != null)
                {
                    throw new Common.Exeptions.DuplicateUserException();
                }
                var user = _mapper.Map<Domain.Entity.User>(createUserCommand);
                await _userRepository.AddAsync(user);
                var userOutPut = _mapper.Map<UserDto>(user);
                return userOutPut;

            }
        }
        public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
        {
            public CreateUserCommandValidator()
            {
                RuleFor(current => current.FullName).NotEmpty().WithMessage("وارد کردن نام و نام خانوادگی اجباری است").
                    Length(6, 50).WithMessage("نام و نام خانوادگی باید بین 6 تا 50 حرف باشد");
                RuleFor(current => current.UserName).NotEmpty().WithMessage("وارد کردن نام کاربری اجباری است").
                    Length(6, 30).WithMessage("نام و نام خانوادگی باید بین 6 تا 30 حرف باشد");
                RuleFor(user => user.Password)
                    .NotEmpty().WithMessage("رمز عبور نمی‌تواند خالی باشد.")
                    .MinimumLength(6).WithMessage("رمز عبور باید حداقل ۶ کاراکتر باشد.")
                    .Matches("[A-Z]").WithMessage("رمز عبور باید حداقل یک حرف بزرگ داشته باشد.")
                    .Matches("[a-z]").WithMessage("رمز عبور باید حداقل یک حرف کوچک داشته باشد.")
                    .Matches("[0-9]").WithMessage("رمز عبور باید حداقل یک عدد داشته باشد.");
                RuleFor(user => user.Email)
                    .NotEmpty().WithMessage("ایمیل نمی‌تواند خالی باشد.")
                    .EmailAddress().WithMessage("فرمت ایمیل نامعتبر است.");
                RuleFor(user => user.Address)
                    .MaximumLength(100).WithMessage("آدرس نمی‌تواند بیشتر از ۱۰۰ کاراکتر باشد.");

            }
        }
    }
}
