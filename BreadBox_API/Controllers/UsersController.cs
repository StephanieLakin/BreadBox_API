using BreadBox_API.Models;
using BreadBox_API.Services;
using BreadBox_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BreadBox_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/1       
        [HttpGet("{id}")]
        public async Task<ActionResult<UserModel>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(UserCreateModel userCreateModel)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(userCreateModel);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT: api/users/1
        [HttpPut("{id}")]
        public async Task<ActionResult<UserModel>> UpdateUser(int id, UserCreateModel userCreateModel)
        {
            try
            {
                var updateUser = await _userService.UpdateUserAsync(id, userCreateModel);
                if (updateUser == null)
                {
                    return NotFound();
                }
                return Ok(updateUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete: api/users/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(deleted);
            }
            return NoContent();
        }

    }
}

