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

        //"userName": "aliybuasini",
        //"password": "niswis@yus",
        //"fullName": "mahdi-maani",
        //"email": "mahdi-maanigmail.com",
        //"address": "hamedan"

        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //[HttpGet("GetAllUser")]
        //public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUser()
        //{
        //    var users = await _userService.GetAllUserAsync();
        //    return Ok(users);
        //}

        //[HttpGet]
        //public async Task<ActionResult<UserDto>> GetUser(GetUserById getUserById)
        //{
        //    var user = await _userService.GetUserAsync(getUserById);
        //    return Ok(user);
        //}

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
            var userDto = await _mediator.Send (updateUserCommand);
            return NoContent();
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
        //{
        //    await _userService.DeleteUserAsync(deleteUserCommand);
        //    return NoContent();
        //}
    }
}
