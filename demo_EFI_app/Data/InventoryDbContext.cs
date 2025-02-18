// Data/InventoryDbContext.cs
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Models;

namespace demo_EFI_app.Data
{
    public class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; } = null!;
        public DbSet<AssetAssignment> AssetAssignments { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Asset>()
                .ToTable("Assets");

            modelBuilder.Entity<AssetAssignment>()
                .ToTable("AssetAssignments");

            modelBuilder.Entity<AssetAssignment>()
                .HasOne(aa => aa.Asset)
                .WithMany(a => a.Assignments)
                .HasForeignKey(aa => aa.AssetID);

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
    }
}