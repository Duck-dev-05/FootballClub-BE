<<<<<<< HEAD
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

=======
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
namespace FootballClub_Backend.Models.Entities;

public class Booking
{
<<<<<<< HEAD
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("userId")]
    public string UserId { get; set; } = string.Empty;

    [BsonElement("facilityId")]
    public string FacilityId { get; set; } = string.Empty;

    [BsonElement("startTime")]
    public DateTime StartTime { get; set; }

    [BsonElement("endTime")]
    public DateTime EndTime { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = "pending";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
=======
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PlayerId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime Date { get; set; }
    public User? User { get; set; }
    public Player? Player { get; set; }
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 