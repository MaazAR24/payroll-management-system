using GenericRepository;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
    }
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(PayrollContext dbContext) : base(dbContext) { }
    }
}
