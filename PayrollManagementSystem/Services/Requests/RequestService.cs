using PayrollManagementSystem.Controllers;
using PayrollManagementSystem.Models;
using PayrollManagementSystem.Repositories;
using PayrollManagementSystem.Services.FileUploader;
using PayrollManagementSystem.Services.Salaries;

namespace PayrollManagementSystem.Services.Requests
{
    public enum RequestStatuses 
    { 
        Pending = 1,
        Approved = 2,
        Transfered = 3,
        Rejected = 4
    }
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IFileService _fileUploader;
        private readonly ISalaryService _salaryService;
        public RequestService(
            IRequestRepository requestRepository,
            ISalaryService salaryService,
            IFileService fileUploader)
        {
            _requestRepository = requestRepository;
            _salaryService = salaryService;
            _fileUploader = fileUploader;
        }

        public async Task<bool> Approve(int id, string type, List<IFormFile> files)
        {
            try
            {
                var request = await _requestRepository.GetByIdAsync(id);

                if (request is null || !request.CanApprove())
                    return false;
                
                UpdateRequestStatus(type, request);

                await _requestRepository.UpdateAsync(request);

                await _fileUploader.Upload((int)ModuleIds.Requests, request.Id, files);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void UpdateRequestStatus(string type, Request request)
        {
            if (type == "Accountant")
                request.IsApprovedByAccountant = true;
            else if (type == "Manager")
                request.IsApprovedByManager = true;

            if (request.IsApprovedByManager == true && request.IsApprovedByAccountant == true)
                request.Status = (int)RequestStatuses.Approved;
        }

        public async Task<bool> ValidateInput(CreateTransferRequest input)
        {
            var salaries = await _salaryService.GetSalaryListByEmployeeId(input.EmployeeId);
            var salaryIds = salaries.Select(sal => sal.Id).ToList();

            var includes = true;

            foreach (var sal in input.SalaryIds) 
            {
                if (!salaryIds.Contains(sal))
                {
                    includes = false;
                    break;
                } 
            }

            return includes;
        }

        public Task<Request> Create(CreateTransferRequest payload)
        {
            var request = new Request
            {
                EmployeeId = payload.EmployeeId,
                IsApprovedByAccountant = false,
                IsApprovedByManager = false,
                Status = (int)RequestStatuses.Pending,
                RequestedSalaries = payload.SalaryIds.Select(e => new RequestedSalary
                {
                    SalaryId = e
                }).ToList()
            };
            return _requestRepository.CreateAsync(request);
        }

        public async Task<bool> Reject(int id) 
        {
            try
            {
                var request = await _requestRepository.GetByIdAsync(id);

                if (request is null || !request.CanReject())
                    return false;

                request.Status = (int)RequestStatuses.Rejected;

                await _requestRepository.UpdateAsync(request);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Transfer(int id)
        {
            try
            {
                var request = await _requestRepository.GetByIdAsync(id);

                if (request is null || !request.CanTransfer())
                    return false;

                request.Status = (int)RequestStatuses.Transfered;
                await _requestRepository.UpdateAsync(request);

                await TransferSalaries(request);

                return true;
            }
            catch { return false; }
        }

        private async Task TransferSalaries(Request request)
        {
            var salaryIds = request.RequestedSalaries.Select(sal => sal.SalaryId).ToList();
            await _salaryService.TransferSalaries(salaryIds);
        }

        public Task<List<TransferRequestDetail>> GetRequestbyEmployeeId(int id)
        {
            return _requestRepository.GetRequestbyEmployeeId(id, (int)RequestStatuses.Pending);
        }

        public Task<List<TransferRequestDetail>> GetApprovedRequestbyEmployeeId(int id)
        {
            return _requestRepository.GetRequestbyEmployeeId(id, (int)RequestStatuses.Approved);
        }

        public Task<List<TransferRequestDetail>> GetTrasnferedRequestbyEmployeeId(int id)
        {
            return _requestRepository.GetRequestbyEmployeeId(id, (int)RequestStatuses.Transfered);
        }

        public Task<List<TransferRequestDetail>> GetRejectedRequestbyEmployeeId(int id)
        {
            return _requestRepository.GetRequestbyEmployeeId(id, (int)RequestStatuses.Rejected);
        }
        
    }
}
