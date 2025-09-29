using Microsoft.EntityFrameworkCore;
using MachineManagement.Core.Entities;

namespace MachineManagement.Infrastructure.Data;

public class MachineManagementDbContext : DbContext
{
    public MachineManagementDbContext(DbContextOptions<MachineManagementDbContext> options) : base(options)
    {
    }

    public DbSet<Buyer> Buyers { get; set; }
    public DbSet<ProductionLine> ProductionLines { get; set; }
    public DbSet<Station> Stations { get; set; }
    public DbSet<Machine> Machines { get; set; }
    public DbSet<MachineLog> MachineLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Buyer
        modelBuilder.Entity<Buyer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Description).HasMaxLength(1000);
        });

        // Configure ProductionLine
        modelBuilder.Entity<ProductionLine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.Buyer)
                .WithMany(e => e.ProductionLines)
                .HasForeignKey(e => e.BuyerId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Station
        modelBuilder.Entity<Station>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(1000);
            
            entity.HasOne(e => e.ProductionLine)
                .WithMany(e => e.Stations)
                .HasForeignKey(e => e.ProductionLineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Machine
        modelBuilder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(50);
            entity.Property(e => e.SerialNumber).IsRequired().HasMaxLength(100);
            entity.HasIndex(e => e.SerialNumber).IsUnique();
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.Manufacturer).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Status).HasConversion<int>();
            
            entity.HasOne(e => e.Station)
                .WithMany(e => e.Machines)
                .HasForeignKey(e => e.StationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure MachineLog
        modelBuilder.Entity<MachineLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LogLevel).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Message).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Details).HasMaxLength(4000);
            entity.Property(e => e.Source).HasMaxLength(255);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.MachineStatusAtTime).HasConversion<int>();
            
            entity.HasOne(e => e.Machine)
                .WithMany(e => e.MachineLogs)
                .HasForeignKey(e => e.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => e.LogLevel);
        });
    }
}