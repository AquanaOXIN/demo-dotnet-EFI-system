using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Models;
using demo_EFI_app.Models.EmployeeDb;
using demo_EFI_app.Models.FinanceDb;

namespace demo_EFI_app.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<AssetAssignment> AssetAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure inventory_db tables
            modelBuilder.Entity<Asset>()
                .ToTable("Assets", "dbo");

            modelBuilder.Entity<AssetAssignment>()
                .ToTable("AssetAssignments", "dbo");

            // Configure employee_db tables
            modelBuilder.Entity<Employee>()
                .ToTable("Employees", "dbo")
                .HasKey(e => e.EmployeeID);

            // Configure finance_db tables
            modelBuilder.Entity<FinanceAsset>()
                .ToTable("Assets", "dbo")
                .HasKey(a => a.AssetID);

            // Configure relationships
            modelBuilder.Entity<AssetAssignment>()
                .HasOne(aa => aa.Asset)
                .WithMany(a => a.Assignments)
                .HasForeignKey(aa => aa.AssetID);

            // Configure string length constraints
            modelBuilder.Entity<Asset>(entity =>
            {
                entity.Property(e => e.SerialNumber).HasMaxLength(100);
                entity.Property(e => e.ModelNumber).HasMaxLength(100);
                entity.Property(e => e.Manufacturer).HasMaxLength(100);
                entity.Property(e => e.Notes).HasMaxLength(500);
            });

            modelBuilder.Entity<AssetAssignment>(entity =>
            {
                entity.Property(e => e.LocationDescription).HasMaxLength(200);
                entity.Property(e => e.AssignmentNotes).HasMaxLength(500);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // Configure different database connections
                var inventoryConnection = "Server=localhost;Database=inventory_db;Trusted_Connection=True;TrustServerCertificate=True";
                var employeeConnection = "Server=localhost;Database=employee_db;Trusted_Connection=True;TrustServerCertificate=True";
                var financeConnection = "Server=localhost;Database=finance_db;Trusted_Connection=True;TrustServerCertificate=True";

                optionsBuilder.UseSqlServer(inventoryConnection);
            }
        }
    }
}