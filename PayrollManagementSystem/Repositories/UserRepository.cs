using GenericRepository;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Repositories
{
    public interface IUserRepository : IRepository<User> 
    {
        Task<User> GetUserAsync(string username, string password);
    }
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly PayrollContext _dbContext;
        public UserRepository(PayrollContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<User> GetUserAsync(string username, string password)
        {
            return await _dbContext.Users
                .Where(us => us.UserName == username && us.Password == password)
                .FirstOrDefaultAsync();
        }
    }
}
