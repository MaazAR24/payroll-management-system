using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PayrollManagementSystem.Services.Requests;

namespace PayrollManagementSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestsController : ControllerBase
    {
        private readonly IRequestService _requestService;
        public RequestsController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [Authorize(Roles = "Accountant")]
        [HttpPost]
        public async Task<ActionResult> CreateRequest(CreateTransferRequest request)
        {
            var validated = await _requestService.ValidateInput(request);

            if(!validated)
                return StatusCode(400, validated);

            var createdRequest = await _requestService.Create(request);
            return StatusCode(201, createdRequest);
        }

        [Authorize]
        [HttpGet("employee-requests/{id}")]
        public async Task<ActionResult> GetRequestsByEmployeeId(int id)
        {
            var requests = await _requestService.GetRequestbyEmployeeId(id);
            if (requests is not null)
                return StatusCode(200, requests);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize]
        [HttpGet("employee-requests-approved/{id}")]
        public async Task<ActionResult> GetApprovedRequestbyEmployeeId(int id)
        {
            var requests = await _requestService.GetApprovedRequestbyEmployeeId(id);
            if (requests is not null)
                return StatusCode(200, requests);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize]
        [HttpGet("employee-requests-transfered/{id}")]
        public async Task<ActionResult> GetTrasnferedRequestbyEmployeeId(int id)
        {
            var requests = await _requestService.GetTrasnferedRequestbyEmployeeId(id);
            if (requests is not null)
                return StatusCode(200, requests);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize]
        [HttpGet("employee-requests-rejected/{id}")]
        public async Task<ActionResult> GetRejectedRequestbyEmployeeId(int id)
        {
            var requests = await _requestService.GetRejectedRequestbyEmployeeId(id);
            if (requests is not null)
                return StatusCode(200, requests);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("accountant-approval/{id}")]
        public async Task<ActionResult> AccountantApproval(int id, [FromForm] List<IFormFile> files)
        {
            var updated = await _requestService.Approve(id, "Accountant", files);
            if (updated)
                return StatusCode(200, true);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize(Roles = "Manager")]
        [HttpPut("manager-approval/{id}")]
        public async Task<ActionResult> ManagerApproval(int id, [FromForm] List<IFormFile> files)
        {
            var updated = await _requestService.Approve(id, "Manager", files);
            if (updated)
                return StatusCode(200, true);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize(Roles = "Manager,Accountant")]
        [HttpPut("Reject/{id}")]
        public async Task<ActionResult> Reject(int id)
        {
            var updated = await _requestService.Reject(id);
            if (updated)
                return StatusCode(200, true);

            return StatusCode(500, "Internal Server Error");
        }

        [Authorize(Roles = "Accountant")]
        [HttpPut("transfer/{id}")]
        public async Task<ActionResult> Transfer(int id)
        {
            var transfered = await _requestService.Transfer(id);
            if (transfered)
                return StatusCode(200, transfered);

            return StatusCode(500, "Internal Server Error");
        }
    }

    public class CreateTransferRequest 
    {
        public int EmployeeId { get; set; }
        public List<int> SalaryIds { get; set; }
    }
}
