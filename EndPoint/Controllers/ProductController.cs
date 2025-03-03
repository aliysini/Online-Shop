using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Product.Commands;

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
        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductCommand createProductCommand)
        {
            var product =await _mediator.Send(createProductCommand);
            return Ok(product);
        }
    }
}
