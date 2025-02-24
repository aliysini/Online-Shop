using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.Application.Commands.User;
using OnlineShop.Application.Dtos.User;
using OnlineShop.Application.Queries.User;
using OnlineShop.Application.Services;
using OnlineShop.Domain.Entity;
using System.Collections.Generic;

namespace EndPoint.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet("getall")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUser()
        {
            var users = await _userService.GetAllUserAsync();
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUser(GetUserById getUserById)
        {
            var user = await _userService.GetUserAsync(getUserById);
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CrateUser(CreateUserCommand command)
        {
            var userDto = await _userService.CreateUserAsync(command);
            return Ok(userDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(UpdateUserCommand updateUserCommand)
        {
            await _userService.UpdateUserAsync(updateUserCommand);
            return NoContent();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand deleteUserCommand)
        {
            await _userService.DeleteUserAsync(deleteUserCommand);
            return NoContent();
        }
    }
}
