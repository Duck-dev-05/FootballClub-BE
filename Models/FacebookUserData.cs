using System.Text.Json.Serialization;

namespace FootballClub_Backend.Models;

public class FacebookUserData
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }

    [JsonPropertyName("email")]
    public required string Email { get; set; }
} 