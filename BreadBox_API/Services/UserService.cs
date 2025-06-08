using BreadBox_API.Data;
using BreadBox_API.Entities;
using BreadBox_API.Models;
using BreadBox_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using FluentValidation;

namespace BreadBox_API.Services
{
    public class UserService : IUserService
    {

        private readonly BreadBoxDbContext _context;
        private readonly IValidator<UserCreateModel> _userCreateValidator;
        

        public UserService(BreadBoxDbContext context, IValidator<UserCreateModel> userCreateValidator)
        {
            _context = context;
            _userCreateValidator = userCreateValidator;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    EmailAddress = u.EmailAddress,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SubscriptionPlan = u.SubscriptionPlan,
                    SubscriptionStartDate = u.SubscriptionStartDate,
                    SubscriptionEndDate = u.SubscriptionEndDate,
                    StripeCustomerId = u.StripeCustomerId,
                }).ToListAsync();
        }

        public async Task<UserModel> GetUserByIdAsync(int id)
        {
            var user = await _context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserModel
                {
                    Id = u.Id,
                    EmailAddress = u.EmailAddress,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SubscriptionPlan = u.SubscriptionPlan,
                    SubscriptionStartDate = u.SubscriptionStartDate,
                    SubscriptionEndDate = u.SubscriptionEndDate,
                    StripeCustomerId = u.StripeCustomerId
                })
                .FirstOrDefaultAsync();
            return user;
        }

        public async Task<UserModel> CreateUserAsync(int id, UserCreateModel userCreateModel)
        {

            var validationResult = await _userCreateValidator.ValidateAsync(userCreateModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }
            // Basic validation: Check if email is unique
            if (await _context.Users.AnyAsync(u => u.EmailAddress == userCreateModel.EmailAddress && u.Id != id))
            {
                throw new ArgumentException("Email address is already in use.");
            }
            var user = new User
            {
                EmailAddress = userCreateModel.EmailAddress,
                PasswordHash = HashPassword(userCreateModel.Password), 
                FirstName = userCreateModel.FirstName,
                LastName = userCreateModel.LastName,
                SubscriptionPlan = userCreateModel.SubscriptionPlan,
                SubscriptionStartDate = null,
                SubscriptionEndDate = null,
                StripeCustomerId = userCreateModel.StripeCustomerId
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserModel
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SubscriptionPlan = user.SubscriptionPlan,
                SubscriptionStartDate = user.SubscriptionStartDate,
                SubscriptionEndDate = user.SubscriptionEndDate,
                StripeCustomerId = user.StripeCustomerId
            };
        }



        public async Task<UserModel> UpdateUserAsync(int id, UserCreateModel userCreateModel)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return null;
            }

            var validationResult = await _userCreateValidator.ValidateAsync(userCreateModel);
            if (!validationResult.IsValid)
            {
                throw new ValidationException(validationResult.Errors);
            }

            if (await _context.Users.AnyAsync(u => u.EmailAddress == userCreateModel.EmailAddress && u.Id != id))
            {
                throw new ArgumentException("Email address is already in use by another user.");
            }

            user.EmailAddress = userCreateModel.EmailAddress;
            user.PasswordHash = HashPassword(userCreateModel.Password);
            user.FirstName = userCreateModel.FirstName;
            user.LastName = userCreateModel.LastName;
            user.SubscriptionPlan = userCreateModel.SubscriptionPlan;
            user.StripeCustomerId = userCreateModel.StripeCustomerId;

            await _context.SaveChangesAsync();

            return new UserModel
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SubscriptionPlan = user.SubscriptionPlan,
                SubscriptionStartDate = user.SubscriptionStartDate,
                SubscriptionEndDate = user.SubscriptionEndDate,
                StripeCustomerId = user.StripeCustomerId
            };
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return false;
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (!string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordHash))
            {
                return false;
            }

            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}
