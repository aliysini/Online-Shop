using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Category;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryCommand createCategoryCommand)
        {
            var category =await _mediator.Send(createCategoryCommand);
            return Ok(category);
        }
    }
}
