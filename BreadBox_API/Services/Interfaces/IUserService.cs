using BreadBox_API.Models;

namespace BreadBox_API.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsersAsync();
        Task<UserModel> GetUserByIdAsync(int id);
        //Task<UserModel> GetByNameAsync(string name);
        Task<UserModel> CreateUserAsync(UserCreateModel userCreateModel);
        Task<UserModel> UpdateUserAsync(int id, UserCreateModel userCreateModel);
        Task<bool> DeleteUserAsync(int id);
    }
}
