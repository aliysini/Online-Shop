using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.Application.Dtos;
using OnlineShop.Domain.Contracts;
using OnlineShop.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Application.Features.Category
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; }
        public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
        {
            public CreateCategoryCommandHandler(IMapper mapper, ICategoryRepository categoryRepository)
            {
                _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
                _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
            }
            private readonly IMapper _mapper;
            private readonly ICategoryRepository _categoryRepository;
            public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
            {
                var validationResult = new CreateCategoryCommandValidator().Validate(request);
                if (!validationResult.IsValid) 
                {
                    var messages = validationResult.Errors.Select(e=>e.ErrorMessage).ToList();
                    throw new Common.Exeptions.ValidationExeption(messages);
                }
                var categoryFromDto = await _categoryRepository.GetByCategoryNameAsync(request.Name);
                if (categoryFromDto != null)
                {
                    throw new Exception($"دسته بندی با اسم {request.Name} قبلا ایجاد شده است ");
                }
                var category = _mapper.Map<Domain.Entity.Category>(request);
                await _categoryRepository.AddAsync(category);
                return _mapper.Map<CategoryDto>(category);

            }
        }
        public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
        {
            public CreateCategoryCommandValidator()
            {
                RuleFor(current => current.Name).NotEmpty().WithMessage("وارد کردن نام دسته بندی الزامی است").
                     Length(3, 50).WithMessage("نام دسته بندی باید بین 3 تا 50 حرف باشد");
            }
        }
    }
}
