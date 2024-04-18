using PayrollManagementSystem.Controllers;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;

namespace PayrollManagementSystem.Services.Requests
{
    public interface IRequestService
    {
        Task<bool> ValidateInput(CreateTransferRequest input);
        Task<Request> Create(CreateTransferRequest request);
        Task<bool> Approve(int id, string type, List<IFormFile> files);
        Task<List<TransferRequestDetail>> GetRequestbyEmployeeId(int id);
        Task<List<TransferRequestDetail>> GetApprovedRequestbyEmployeeId(int id);
        Task<List<TransferRequestDetail>> GetTrasnferedRequestbyEmployeeId(int id);
        Task<List<TransferRequestDetail>> GetRejectedRequestbyEmployeeId(int id);
        Task<bool> Reject(int id);
        Task<bool> Transfer(int id);
    }
}
