using PayrollManagementSystem.Services.Requests;

namespace PayrollManagementSystem.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public bool? IsApprovedByAccountant { get; set; }
        public bool? IsApprovedByManager { get; set; }
        public int Status { get; set; }
        public virtual ICollection<RequestedSalary> RequestedSalaries { get; set; }
        public bool CanApprove() => Status == (int)RequestStatuses.Approved || Status == (int)RequestStatuses.Pending;
        public bool CanReject() => Status == (int)RequestStatuses.Pending;
        public bool CanTransfer() => Status == (int)RequestStatuses.Approved;
    }
    public class RequestedSalary
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public int SalaryId { get; set; }
        public virtual Salary Salary { get;}
    }

    public class RequestFiles
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string FileName { get; set; }
    }
}
