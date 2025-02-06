using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Models.Responses;

public class UserDto
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Provider { get; set; }

    public UserDto() { }

    public UserDto(UserEntity user)
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Role = user.Role;
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        Provider = user.Provider;
    }
} 