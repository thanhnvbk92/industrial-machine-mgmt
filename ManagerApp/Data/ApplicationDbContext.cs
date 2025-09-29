using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ManagerApp.Models;

namespace ManagerApp.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Machine> Machines { get; set; }
    public DbSet<LogEntry> LogEntries { get; set; }
    public DbSet<Command> Commands { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure Machine entity
        builder.Entity<Machine>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            entity.Property(e => e.Status)
                .HasConversion<string>();
        });

        // Configure LogEntry entity
        builder.Entity<LogEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => new { e.MachineId, e.Timestamp });
            entity.Property(e => e.Level)
                .HasConversion<string>();
            
            entity.HasOne(e => e.Machine)
                .WithMany(m => m.LogEntries)
                .HasForeignKey(e => e.MachineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Command entity
        builder.Entity<Command>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.MachineId, e.Status });
            entity.Property(e => e.Status)
                .HasConversion<string>();
            
            entity.HasOne(e => e.Machine)
                .WithMany(m => m.Commands)
                .HasForeignKey(e => e.MachineId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}