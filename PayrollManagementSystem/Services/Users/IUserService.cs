using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Services.Users
{
    public interface IUserService
    {
        Task<User> GetUser(string username, string password);
    }
}
