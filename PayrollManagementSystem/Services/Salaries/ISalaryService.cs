using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Services.Salaries
{
    public interface ISalaryService
    {
        Task<Salary> AddEmployeeSalary(int employeeId, AddSalary obj);
        Task<List<Salary>> GetSalaryListByEmployeeId(int employeeId);
        Task<List<Salary>> GetTransferedSalaryListByEmployeeId(int employeeId);
        Task<bool> TransferSalaries(List<int> ids);
    }
}
