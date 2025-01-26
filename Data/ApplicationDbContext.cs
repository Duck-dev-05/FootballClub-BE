using Microsoft.EntityFrameworkCore;
using FootballClub_Backend.Models;

namespace FootballClub_Backend.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Player> Players { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<UserAuditLog> UserAuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User entity
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasDefaultValue("user");

        // Configure UserAuditLog entity
        modelBuilder.Entity<UserAuditLog>()
            .HasIndex(l => l.UserId);

        // Configure relationships
        modelBuilder.Entity<Booking>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(b => b.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Booking>()
            .HasOne<Player>()
            .WithMany(p => p.Bookings)
            .HasForeignKey(b => b.PlayerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
} 