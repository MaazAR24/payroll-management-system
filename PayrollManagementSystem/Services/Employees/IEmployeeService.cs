using PayrollManagementSystem.Models;
using PayrollManagementSystem.Services.Salaries;

namespace PayrollManagementSystem.Services.Employees
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task<Employee> CreateEmployeeAsync(Employee employee);
        Task<Employee> UpdateEmployeeAsync(int id, Employee employee);
        Task<bool> DeleteEmployeeAsync(int id);
        Task<Salary> AddSalary(int id, AddSalary salary);
        Task<List<Salary>> GetSalaryList(int id);
        Task<List<Salary>> GetTransferedSalaryList(int id);
    }
}
