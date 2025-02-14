// Data/ApplicationDbContext.cs
using System;
using Microsoft.EntityFrameworkCore;
using demo_EFI_app.Models;

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
            // Configure the Asset table
            modelBuilder.Entity<Asset>()
                .ToTable("Assets");

            // Configure the AssetAssignment table
            modelBuilder.Entity<AssetAssignment>()
                .ToTable("AssetAssignments");

            // Configure relationships
            modelBuilder.Entity<AssetAssignment>()
                .HasOne(aa => aa.Asset)
                .WithMany(a => a.Assignments)
                .HasForeignKey(aa => aa.AssetID);

            // Ensure required string properties have max length
            modelBuilder.Entity<Asset>()
                .Property(a => a.SerialNumber)
                .HasMaxLength(100);

            modelBuilder.Entity<Asset>()
                .Property(a => a.ModelNumber)
                .HasMaxLength(100);

            modelBuilder.Entity<Asset>()
                .Property(a => a.Manufacturer)
                .HasMaxLength(100);

            modelBuilder.Entity<AssetAssignment>()
                .Property(aa => aa.LocationDescription)
                .HasMaxLength(200);
        }
    }
}