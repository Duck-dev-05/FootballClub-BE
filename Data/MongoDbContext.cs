using Microsoft.EntityFrameworkCore;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Models.Config;
using Microsoft.Extensions.Options;

namespace FootballClub_Backend.Data;

public class MongoDbContext : DbContext
{
    public MongoDbContext(DbContextOptions<MongoDbContext> options) : base(options) { }

    public DbSet<UserEntity> Users { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<CalendarEvent> CalendarEvents { get; set; }
    public DbSet<Gallery> Galleries { get; set; }

    public IMongoCollection<Player> Players => _database.GetCollection<Player>("Players");
    public IMongoCollection<Match> Matches => _database.GetCollection<Match>("Matches");
    public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("Tickets");
    public IMongoCollection<UserAuditLog> UserAuditLogs => _database.GetCollection<UserAuditLog>("UserAuditLogs");

    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbConfig> config)
    {
        var client = new MongoClient(config.Value.ConnectionString);
        _database = client.GetDatabase(config.Value.DatabaseName);
    }
} 