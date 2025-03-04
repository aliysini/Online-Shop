using AutoMapper;
using FluentValidation;
using MediatR;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.User.Commands
{
    public class UpdateUserCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public class HandlerUpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
        {
            private readonly IMapper _mapper;

            private readonly IUserRepository _userRepository;
            public HandlerUpdateUserCommandHandler(IMapper mapper,IUserRepository userRepository)
            {
                _userRepository = userRepository;   
                _mapper = mapper;
            }
            public async Task<Unit> Handle(UpdateUserCommand updateUserCommand,CancellationToken cancellationToken)
            {
                var validationResult = await new UpdateUserCommandValidator().ValidateAsync(updateUserCommand);
                if (!validationResult.IsValid)
                {
                    var errorMessages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(errorMessages);
                }
                var userFromDataBase = await _userRepository.GetByIdAsync(updateUserCommand.Id);
                if (userFromDataBase == null)
                {
                    throw new Exception("کاربر مورد نظر پیدا نشد.");
                }
                updateUserCommand.Id = userFromDataBase.Id;
                var user = _mapper.Map(updateUserCommand,userFromDataBase);
                await _userRepository.UpdateAsync(userFromDataBase);
                return Unit.Value;
            }
        }
        public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
        {
            public UpdateUserCommandValidator()
            {
                RuleFor(current => current.FullName).NotEmpty().WithMessage("وارد کردن نام و نام خانوادگی اجباری است").
                    Length(6, 50).WithMessage("نام و نام خانوادگی باید بین 6 تا 50 حرف باشد");
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
