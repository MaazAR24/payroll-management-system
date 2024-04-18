using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;
using PayrollManagementSystem.Services.Salaries;

namespace PayrollManagementSystem.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ISalaryService _salaryService;

        public EmployeeService(IEmployeeRepository employeeRepository, ISalaryService salaryService)
        {
            _employeeRepository = employeeRepository;
            _salaryService = salaryService;
        }

        public Task<List<Employee>> GetAllEmployeesAsync()
        {
            return _employeeRepository.ListAsync();
        }

        public Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return _employeeRepository.GetByIdAsync(id);
        }

        public Task<Employee> CreateEmployeeAsync(Employee employee)
        {
            return _employeeRepository.CreateAsync(employee);
        }

        public async Task<Employee> UpdateEmployeeAsync(int id, Employee employee)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
                return null;

            existingEmployee.Name = employee.Name;
            existingEmployee.Position = employee.Position;
            existingEmployee.Department = employee.Department;
            return await _employeeRepository.UpdateAsync(existingEmployee);
        }

        public Task<bool> DeleteEmployeeAsync(int id)
        {
            return _employeeRepository.DeleteAsync(id);
        }

        public Task<Salary> AddSalary(int id, AddSalary salary)
        {
            return _salaryService.AddEmployeeSalary(id, salary);
        }

        public Task<List<Salary>> GetSalaryList(int id)
        {
            return _salaryService.GetSalaryListByEmployeeId(id);
        }

        public Task<List<Salary>> GetTransferedSalaryList(int id)
        {
            return _salaryService.GetTransferedSalaryListByEmployeeId(id);
        }
    }
}
