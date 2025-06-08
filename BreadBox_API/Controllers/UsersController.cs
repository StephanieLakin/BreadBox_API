using BreadBox_API.Models;
using BreadBox_API.Services;
using BreadBox_API.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using BreadBox_API.Data;

namespace BreadBox_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        private readonly BreadBoxDbContext _context;

        public UsersController(IUserService userService, IConfiguration configuration, BreadBoxDbContext context)
        {
            _userService = userService;
            _configuration = configuration;
            _context = context;
        }

        // GET: api/users
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET: api/users/1       
        [HttpGet("{id}")]
        [Authorize]
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
        public async Task<ActionResult<UserModel>> CreateUser(int id, UserCreateModel userCreateModel)
        {
            try
            {
                var createdUser = await _userService.CreateUserAsync(id, userCreateModel);
                return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (ArgumentException ex)
            {

                return BadRequest(ex.Message);
            }
        }

        // PUT: api/users/1
        [HttpPut("{id}")]
        [Authorize]
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
            catch (ValidationException ex)
            {
                return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // Delete: api/users/1
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserAsync(id);
            if (!deleted)
            {
                return NotFound(deleted);
            }
            return NoContent();
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login(LoginModel loginModel)
        //{
        //    var user = await _context.Users
        //        .Where(u => u.EmailAddress.ToLower() == loginModel.EmailAddress.ToLower()) // Case-insensitive
        //        .Select(u => new UserModel
        //        {
        //            Id = u.Id,
        //            EmailAddress = u.EmailAddress,
        //            PasswordHash = u.PasswordHash, // Use PasswordHash
        //            FirstName = u.FirstName,
        //            LastName = u.LastName,
        //            SubscriptionPlan = u.SubscriptionPlan,
        //            StripeCustomerId = u.StripeCustomerId
        //        })
        //        .FirstOrDefaultAsync();

        //    if (user == null || !_userService.VerifyPassword(loginModel.Password, user.PasswordHash)) // Use PasswordHash
        //    {
        //        return Unauthorized(new { Errors = new List<string> { "Invalid email or password." } });
        //    }



        //        
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if (string.IsNullOrWhiteSpace(loginModel.EmailAddress) || string.IsNullOrWhiteSpace(loginModel.Password))
            {
                return BadRequest(new { Errors = new List<string> { "Email and password are required." } });
            }

            var userEntity = await _context.Users
                .FirstOrDefaultAsync(u => u.EmailAddress.ToLower() == loginModel.EmailAddress.ToLower());

            if (userEntity == null || !_userService.VerifyPassword(loginModel.Password, userEntity.PasswordHash))
            {
                return Unauthorized(new { Errors = new List<string> { "Invalid email or password." } });
            }

            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, userEntity.Id.ToString()),
        new Claim(ClaimTypes.Email, userEntity.EmailAddress),
        new Claim(ClaimTypes.Role, "User")
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new
            {
                Token = tokenString,
                User = new
                {
                    userEntity.Id,
                    userEntity.EmailAddress,
                    userEntity.FirstName,
                    userEntity.LastName,
                    userEntity.SubscriptionPlan,
                    userEntity.SubscriptionStartDate,
                    userEntity.SubscriptionEndDate,
                    userEntity.StripeCustomerId
                }
            });
        }
    }
}

