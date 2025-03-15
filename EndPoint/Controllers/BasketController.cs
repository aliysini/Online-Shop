using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.Baskets.Commands;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        public BasketController(IMediator mediator)
        {
            _mediator = mediator;
        }
        private readonly IMediator _mediator;
        [HttpPost]
        public async Task<ActionResult<ShoppingCartDto>> AddBasket(CreateBasketCommand createBasketCommand)
        {
            var basket = await _mediator.Send(createBasketCommand);
            return Ok(basket);

        }
    }
}
