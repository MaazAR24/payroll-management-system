namespace PayrollManagementSystem.Models
{
    public class Salary
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Amount { get; set; }
        public bool? Transfered { get; set; }
    }
}
