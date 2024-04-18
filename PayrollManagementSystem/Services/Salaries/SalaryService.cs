using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;

namespace PayrollManagementSystem.Services.Salaries
{
    public class SalaryService : ISalaryService
    {
        private readonly ISalaryRepository _salaryRepository;
        public SalaryService(ISalaryRepository salaryRepository)
        {
            _salaryRepository = salaryRepository;
        }
        public async Task<Salary> AddEmployeeSalary(int employeeId, AddSalary salary)
        {
            var existingSalary = await _salaryRepository.GetExistingSalary(employeeId, salary.Month, salary.Year);
            if (existingSalary is not null)
            {
                existingSalary.Amount = salary.Amount;
                return await _salaryRepository.UpdateAsync(existingSalary);
            }

            var newSalary = new Salary
            {
                EmployeeId = employeeId,
                Month = salary.Month,
                Year = salary.Year,
                Amount = salary.Amount,
                Transfered = false
            };
            return await _salaryRepository.CreateAsync(newSalary);
        }

        public Task<List<Salary>> GetSalaryListByEmployeeId(int employeeId)
        {
            return _salaryRepository.GetSalaryListByEmployeeId(employeeId, false);
        }

        public Task<List<Salary>> GetTransferedSalaryListByEmployeeId(int employeeId)
        {
            return _salaryRepository.GetSalaryListByEmployeeId(employeeId, true);
        }

        public async Task<bool> TransferSalaries(List<int> ids)
        {
            var salaries = await _salaryRepository.GetByIdsAsync(ids);
            
            salaries.ForEach(s => { s.Transfered = true; });
            
            await _salaryRepository.PersistChanges();

            return true;
        }
    }
}
