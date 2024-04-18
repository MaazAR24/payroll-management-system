using GenericRepository;
using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Data;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Services.Requests;
using System;

namespace PayrollManagementSystem.Repositories
{
    public interface IRequestRepository : IRepository<Request>
    {
        Task<List<TransferRequestDetail>> GetRequestbyEmployeeId(int id, int status);
        //Task<Request> GetRequestWithSalaries(int id);
    }
    public class RequestRepository : Repository<Request>, IRequestRepository
    {
        private readonly PayrollContext _dbContext;
        public RequestRepository(PayrollContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<List<TransferRequestDetail>> GetRequestbyEmployeeId(int id, int status)
        {
            return await _dbContext.Requests
                .Where(e => e.EmployeeId == id && e.Status == status)
                .Include(r => r.RequestedSalaries)
                .Select(req => new TransferRequestDetail {
                    Id = req.Id,
                    EmployeeId = req.EmployeeId,
                    IsApprovedByAccountant = req.IsApprovedByAccountant,
                    IsApprovedByManager = req.IsApprovedByManager,
                    Status = ((RequestStatuses)req.Status).ToString(),
                    SalaryDetails = req.RequestedSalaries.Select(s => new RequestSalaryDetail
                    {
                        Month = s.Salary.Month,
                        Year = s.Salary.Year,
                        Amount = s.Salary.Amount

                    }).ToList()
                })
                .ToListAsync();
        }

        public override async Task<Request> GetByIdAsync(int id) 
        {
            return await _dbContext.Requests
                .Where(r => r.Id == id)
                .Include(i => i.RequestedSalaries)
                .FirstOrDefaultAsync();
        }

        public async Task<Request> GetRequestWithSalaries(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class TransferRequestDetail
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool? IsApprovedByAccountant { get; set; }
        public bool? IsApprovedByManager { get; set; }
        public string Status { get; set; }
        public List<RequestSalaryDetail> SalaryDetails { get; set; }
    }

    public class RequestSalaryDetail
    {
        public int Month { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
    }
}
