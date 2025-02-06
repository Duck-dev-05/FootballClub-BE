<<<<<<< HEAD
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

=======
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
namespace FootballClub_Backend.Models.Entities;

public class UserAuditLog
{
<<<<<<< HEAD
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("action")]
    public string Action { get; set; } = string.Empty;

    [BsonElement("details")]
    public string Details { get; set; } = string.Empty;

    [BsonElement("timestamp")]
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
=======
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public DateTime Timestamp { get; set; }
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 