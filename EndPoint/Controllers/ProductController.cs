using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Product.Commands;
using OnlineShop.Application.Features.Product.Queries;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetAllProduct()
        {
            var pro = new GetAllProductQuery();
            var products = await _mediator.Send(pro);
            return products;
        }  
        [HttpGet("{name}")]
        public async Task<ActionResult<ProductDto>> GetProductByName(string name)
        {
            var request =new GetProductByNameQuery() { Name = name };
            return await _mediator.Send(request);

        }
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductCommand createProductCommand)
        {
            var product =await _mediator.Send(createProductCommand);
            return Ok(product);
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand updateProductCommand)
        {
            await _mediator.Send(updateProductCommand);
            return NoContent();
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(DeleteProductCommand deleteProductCommand)
        {
            await _mediator.Send(deleteProductCommand);
            return NoContent();
        }
    }
}
