using GenericRepository;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Repositories
{
    public interface ISalaryRepository : IRepository<Salary>
    {
        Task<List<Salary>> GetSalaryListByEmployeeId(int employeeId, bool transfered);
        Task<Salary> GetExistingSalary(int employeeId, int month, int year);
        Task<List<Salary>> GetByIdsAsync(List<int> ids);
    }
    public class SalaryRepository : Repository<Salary>, ISalaryRepository
    {
        private readonly PayrollContext _dbContext;

        public SalaryRepository(PayrollContext dbContext) : base(dbContext) {
            _dbContext = dbContext;
        }

        public async Task<Salary> GetExistingSalary(int employeeId, int month, int year)
        {
            return await _dbContext.Salaries.Where(
                                        sal => sal.EmployeeId == employeeId 
                                        && sal.Month == month 
                                        && sal.Year == year
                                        ).FirstOrDefaultAsync();
        }

        public async Task<List<Salary>> GetSalaryListByEmployeeId(int employeeId, bool transfered)
        {
            if (transfered) 
                return await _dbContext.Salaries
                    .Where(sal => 
                        sal.EmployeeId == employeeId 
                        && sal.Transfered == true 
                        )
                    .ToListAsync();
            else
                return await _dbContext.Salaries
                    .Where(sal =>
                        sal.EmployeeId == employeeId
                        && sal.Transfered != true
                        )
                    .ToListAsync();
        }

        public async Task<List<Salary>> GetByIdsAsync(List<int> ids)
        {
            return await _dbContext.Salaries
                .Where(e => ids.Contains(e.Id))
                .ToListAsync();
        }
    }
}
