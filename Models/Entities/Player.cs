<<<<<<< HEAD
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
=======
using System.ComponentModel.DataAnnotations;
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b

namespace FootballClub_Backend.Models.Entities;

public class Player
{
<<<<<<< HEAD
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
=======
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 