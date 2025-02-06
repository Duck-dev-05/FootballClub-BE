using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FootballClub_Backend.Models.Entities;

public class Match
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("homeTeam")]
    public string HomeTeam { get; set; } = string.Empty;

    [BsonElement("awayTeam")]
    public string AwayTeam { get; set; } = string.Empty;

    [BsonElement("date")]
    public DateTime Date { get; set; }

    [BsonElement("venue")]
    public string Venue { get; set; } = string.Empty;

    [BsonElement("competition")]
    public string Competition { get; set; } = string.Empty;

    [BsonElement("status")]
    public string Status { get; set; } = "scheduled"; // scheduled, live, completed, cancelled

    [BsonElement("score")]
    public MatchScore? Score { get; set; }

    [BsonElement("ticketsAvailable")]
    public int TicketsAvailable { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}

public class MatchScore
{
    [BsonElement("homeScore")]
    public int HomeScore { get; set; }

    [BsonElement("awayScore")]
    public int AwayScore { get; set; }
} 