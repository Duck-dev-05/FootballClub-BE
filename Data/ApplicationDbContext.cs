using Microsoft.EntityFrameworkCore;
using FootballClub_Backend.Models.Entities;

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
    public DbSet<News> News { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<Match> Matches { get; set; }
    public DbSet<Gallery> Gallery { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // Configure User entity
        modelBuilder.Entity<User>()
            .Property(u => u.Role)
            .HasDefaultValue("user");

        // Configure decimal precision
        modelBuilder.Entity<Match>()
            .Property(m => m.TicketPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Player>()
            .Property(p => p.Price)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Ticket>()
            .Property(t => t.Price)
            .HasPrecision(10, 2);

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

        modelBuilder.Entity<Ticket>()
            .HasOne<Match>()
            .WithMany()
            .HasForeignKey(t => t.MatchId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Ticket>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CalendarEvent>()
            .HasOne(e => e.Creator)
            .WithMany()
            .HasForeignKey(e => e.CreatedBy)
            .OnDelete(DeleteBehavior.Restrict);
    }
} 