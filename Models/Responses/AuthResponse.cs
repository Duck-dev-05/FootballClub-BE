using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Responses;

public class AuthResponse
{
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = null!;
} 