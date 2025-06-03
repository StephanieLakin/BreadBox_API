using BreadBox_API.Data;
using BreadBox_API.Entities;
using BreadBox_API.Models;
using BreadBox_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BreadBox_API.Services
{
    public class UserService : IUserService
    {

        private readonly BreadBoxDbContext _context;

        public UserService(BreadBoxDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserModel>> GetAllUsersAsync()
        {
            return await _context.Users
                .Select (u => new UserModel
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

        public async Task<UserModel> GetByIdAsync(int id)
        {
            var user = await _context.Users
                .Where (u => u.Id == id)
                .Select (u => new UserModel
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

        public async Task<UserModel> CreateUserAsync(UserCreateModel userCreateModel)
        {
            // Basic validation: Check if email is unique
            if (await _context.Users.AnyAsync(u => u.EmailAddress == userCreateModel.EmailAddress))
            {
                throw new ArgumentException("Email address is already in use.");
            }
            var user = new User
            {
                EmailAddress = userCreateModel.EmailAddress,
                PasswordHash = HashPassword(userCreateModel.Password), // Placeholder for hashing
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



        public Task<UserModel> UpdateUserAsync(UserCreateModel userCreateModel)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        private string HashPassword(string password)
        {
            // Placeholder: In completed app, will use a proper hashing library like BCrypt
            return password + "_hashed"; // Temporary implementation
        }
    }
}
