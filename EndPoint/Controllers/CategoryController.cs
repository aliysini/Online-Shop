using MediatR;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Category.Commands;
using OnlineShop.Application.Features.Category.Queries;

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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategory()
        {
            var request = new GetAllCategoryQuerirs();
            var categories = await _mediator.Send(request);
            return Ok(categories);
        }
        [HttpGet("{name}")]
        public async Task<ActionResult<CategoryDto>> GetCategory(string name)
        {
            var request = new GetCategoryQuery { Name = name };
            var category = await _mediator.Send(request);
            return Ok(category);
        }
        [HttpPost]
        public async Task<ActionResult<CategoryDto>> CreateCategory(CreateCategoryCommand createCategoryCommand)
        {
            var category =await _mediator.Send(createCategoryCommand);
            return Ok(category);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand updateCategoryCommand)
        {
            await _mediator.Send(updateCategoryCommand);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteCategory(DeleteCategoryCommand deleteCategoryCommand)
        {
            await _mediator.Send(deleteCategoryCommand);
            return NoContent();
        }
    }
}
