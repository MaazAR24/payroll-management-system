using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;

namespace PayrollManagementSystem.Services.Users
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public Task<User> GetUser(string username, string password)
        {
            return _userRepository.GetUserAsync(username, password);
        }
    }
}
