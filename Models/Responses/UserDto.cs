using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Models.Responses;

public class UserDto
{
<<<<<<< HEAD
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Provider { get; set; }

    public UserDto() { }

    public UserDto(UserEntity user)
=======
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;

    public UserDto() { }

    public UserDto(User user)
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
    {
        Id = user.Id;
        Username = user.Username;
        Email = user.Email;
        Role = user.Role;
<<<<<<< HEAD
        CreatedAt = user.CreatedAt;
        UpdatedAt = user.UpdatedAt;
        Provider = user.Provider;
=======
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
    }
} 