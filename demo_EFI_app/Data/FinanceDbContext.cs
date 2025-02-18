// Data/FinanceDbContext.cs
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Models.FinanceDb;

namespace demo_EFI_app.Data
{
    public class FinanceDbContext : DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options)
            : base(options)
        {
        }

        public DbSet<FinanceAsset> Assets { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FinanceAsset>(entity =>
            {
                entity.ToTable("Assets");
                entity.HasKey(e => e.AssetID);
                
                entity.Property(e => e.PurchaseOrderNumber).HasMaxLength(50);
                entity.Property(e => e.VendorName).HasMaxLength(100);
                entity.Property(e => e.ActualCost).HasColumnType("decimal(15,2)");
            });
        }
    }
}