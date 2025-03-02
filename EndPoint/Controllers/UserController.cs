using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Dtos;
using OnlineShop.Application.Features.User.Commands;
using OnlineShop.Application.Features.User.Queries;
using OnlineShop.Domain.Entity;
using System.Collections.Generic;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllUser")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUser()
        {
            var getAllUser = new GetAllUser();
            var users = await _mediator.Send(getAllUser);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var getUserByid = new GetUserById { Id = id };
            var user = await _mediator.Send(getUserByid);
            return Ok(user);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserCommand command)
        {
            var userDto = await _mediator.Send(command);
            return Ok(userDto);
            //createatroute
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
        {
            await _mediator.Send (updateUserCommand);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            await _mediator.Send(deleteUserCommand);
            return NoContent();
        }
    }
}
