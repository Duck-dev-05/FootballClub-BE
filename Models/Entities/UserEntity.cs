using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Entities;

public class UserEntity
{
    public int Id { get; set; }

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string PasswordHash { get; set; } = string.Empty;

    public string Role { get; set; } = "user";

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime? UpdatedAt { get; set; }

    [BsonElement("provider")]
    public string? Provider { get; set; }

    [BsonElement("providerId")]
    public string? ProviderId { get; set; }

    [BsonElement("isEmailVerified")]
    public bool IsEmailVerified { get; set; }

    [BsonElement("lastLoginAt")]
    public DateTime? LastLoginAt { get; set; }

    [BsonElement("status")]
    public string Status { get; set; } = "active"; // active, suspended, deleted
} 