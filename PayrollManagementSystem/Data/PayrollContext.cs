using Microsoft.EntityFrameworkCore;
using PayrollManagementSystem.Models;

namespace PayrollManagementSystem.Data
{
    public class PayrollContext : DbContext
    {
        public PayrollContext(DbContextOptions<PayrollContext> options) : base(options){}

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<RequestedSalary> RequestedSalaries { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<FileEntity> Files { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Property Configurations
            modelBuilder.Entity<Request>().HasMany(m => m.RequestedSalaries).WithOne().HasForeignKey(k => k.RequestId);
            modelBuilder.Entity<RequestedSalary>().HasOne(m => m.Salary).WithMany().HasForeignKey(k => k.SalaryId);
        }
    }
}
