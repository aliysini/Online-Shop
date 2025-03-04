using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore.Update.Internal;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static OnlineShop.Application.Features.Category.Commands.CreateCategoryCommand;

namespace OnlineShop.Application.Features.Category.Commands
{
    public class UpdateCategoryCommand : IRequest<Unit>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Unit>
        {
            public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
            {
                _categoryRepository = categoryRepository;
                _mapper = mapper;
            }
            private readonly ICategoryRepository _categoryRepository;
            private readonly IMapper _mapper;
            public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
            {
                var validationResult = new UpdateCategoryCommandValidator().Validate(request);
                if (!validationResult.IsValid)
                {
                    var messages = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(messages);
                }
                var categoryFromDB = await _categoryRepository.GetByIdAsync(request.Id);
                if (categoryFromDB == null)
                {
                    throw new Exception($"دسته بندی ای با شناسه {request.Id} پیدا نشد");
                }
                var categoryDto = _mapper.Map(request, categoryFromDB);
                await _categoryRepository.UpdateAsync(categoryDto);
                return Unit.Value;
            }
        }
        public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
        {
            public UpdateCategoryCommandValidator()
            {
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام دسته بندی الزامی است").
                     Length(3, 50).WithMessage("نام دسته بندی باید بین 3 تا 50 حرف باشد");
                RuleFor(current => current.Id).NotEmpty().WithMessage("وارد کردن شناسه دسته بندی الزامی است")
                    .GreaterThan(0).WithMessage("شناسه دسته بندی باید عددی بالاتر از صفر باشد");
            }
        }
    }
}
