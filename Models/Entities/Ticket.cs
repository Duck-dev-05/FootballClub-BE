<<<<<<< HEAD
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

=======
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
namespace FootballClub_Backend.Models.Entities;

public class Ticket
{
<<<<<<< HEAD
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("matchId")]
    public string MatchId { get; set; } = string.Empty;

    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("section")]
    public string Section { get; set; } = string.Empty;

    [BsonElement("seat")]
    public string Seat { get; set; } = string.Empty;

    [BsonElement("price")]
    public decimal Price { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = "reserved"; // reserved, paid, cancelled

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
=======
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MatchId { get; set; }
    public decimal Price { get; set; }
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 