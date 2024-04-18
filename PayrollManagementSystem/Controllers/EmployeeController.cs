using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Services.Employees;
using PayrollManagementSystem.Services.Salaries;

namespace PayrollManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployeesAsync();
            return Ok(employees);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(int id)
        {
            var employee = await _employeeService.GetEmployeeByIdAsync(id);
            if (employee == null)
                return NotFound();

            return Ok(employee);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee(Employee employee)
        {
            var createdEmployee = await _employeeService.CreateEmployeeAsync(employee);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = createdEmployee.Id }, createdEmployee);
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateEmployee(int id, Employee employee)
        {
            var result = await _employeeService.UpdateEmployeeAsync(id, employee);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            var result = await _employeeService.DeleteEmployeeAsync(id);
            if (!result)
                return NotFound();

            return Ok(result);
        }

        [Authorize(Roles = "Accountant")]
        [HttpPost("{id}/add-salary")]
        public async Task<ActionResult<Salary>> AddSalary(int id, AddSalary salary)
        {
            var addedSalary = await _employeeService.AddSalary(id, salary);
            return CreatedAtAction(nameof(GetEmployeeById), new { id = id }, addedSalary);
        }

        [Authorize]
        [HttpGet("{id}/salary-list")]
        public async Task<ActionResult<Salary>> GetSalaryList(int id)
        {
            var salaries = await _employeeService.GetSalaryList(id);
            return Ok(salaries);
        }

        [Authorize]
        [HttpGet("{id}/transfered-salary-list")]
        public async Task<ActionResult<Salary>> GetTrasnferedSalaryList(int id)
        {
            var salaries = await _employeeService.GetTransferedSalaryList(id);
            return Ok(salaries);
        }
    }
}
