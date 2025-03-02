using FluentValidation;
using MediatR;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.User.Commands
{
    public class DeleteUserCommand :IRequest<Unit>
    {
        public int Id { get; set; }
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
        {
            public DeleteUserCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            private readonly IUserRepository _userRepository;
            public async Task<Unit> Handle(DeleteUserCommand command,CancellationToken cancellationToken)
            {
                var validationUser = await new DeleteUserCommandValidation().ValidateAsync(command);
                if (!validationUser.IsValid)
                {
                    var errorMassage = validationUser.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(errorMassage);
                }
                var userFromDB = await _userRepository.GetByIdAsync(command.Id);
                if (userFromDB == null) 
                {
                    throw new Exception("کاربر مورد نظر پیدا نشد");
                }
                await _userRepository.DeleteAsync(userFromDB);
                return Unit.Value;

            }
        }
        public class DeleteUserCommandValidation : AbstractValidator<DeleteUserCommand>
        {
            public DeleteUserCommandValidation()
            {
                RuleFor(current => current.Id).NotEmpty().WithMessage("وارد کردن شناسه کاربری اجباری است");
            }
        }

    }
}
