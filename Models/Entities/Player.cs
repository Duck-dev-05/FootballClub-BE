using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FootballClub_Backend.Models.Entities;

public class Player
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("position")]
    public string Position { get; set; } = string.Empty;

    [BsonElement("number")]
    public int Number { get; set; }

    [BsonElement("nationality")]
    public string Nationality { get; set; } = string.Empty;

    [BsonElement("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    [BsonElement("imageUrl")]
    public string? ImageUrl { get; set; }

    [BsonElement("stats")]
    public PlayerStats Stats { get; set; } = new();

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class PlayerStats
{
    [BsonElement("appearances")]
    public int Appearances { get; set; }

    [BsonElement("goals")]
    public int Goals { get; set; }

    [BsonElement("assists")]
    public int Assists { get; set; }

    [BsonElement("cleanSheets")]
    public int CleanSheets { get; set; }
} 