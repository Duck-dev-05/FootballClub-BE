using MongoDB.Driver;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Models.Config;
using Microsoft.Extensions.Options;

namespace FootballClub_Backend.Data;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IOptions<MongoDbConfig> config)
    {
        var client = new MongoClient(config.Value.ConnectionString);
        _database = client.GetDatabase(config.Value.DatabaseName);
    }

    public IMongoCollection<UserEntity> Users => _database.GetCollection<UserEntity>("Users");
    public IMongoCollection<Player> Players => _database.GetCollection<Player>("Players");
    public IMongoCollection<Booking> Bookings => _database.GetCollection<Booking>("Bookings");
    public IMongoCollection<Match> Matches => _database.GetCollection<Match>("Matches");
    public IMongoCollection<Ticket> Tickets => _database.GetCollection<Ticket>("Tickets");
    public IMongoCollection<Gallery> Gallery => _database.GetCollection<Gallery>("Gallery");
    public IMongoCollection<CalendarEvent> Events => _database.GetCollection<CalendarEvent>("Events");
    public IMongoCollection<UserAuditLog> UserAuditLogs => _database.GetCollection<UserAuditLog>("UserAuditLogs");
} 