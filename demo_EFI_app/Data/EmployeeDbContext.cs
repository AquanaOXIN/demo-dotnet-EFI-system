// Data/EmployeeDbContext.cs
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Models.EmployeeDb;

namespace demo_EFI_app.Data
{
    public class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .ToTable("Employees");
        }
    }
}