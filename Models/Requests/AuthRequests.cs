using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Requests;

public class RegisterRequest
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = string.Empty;
}

public class LoginRequest
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}

public class GoogleAuthRequest
{
    [Required]
    public string Credential { get; set; } = string.Empty;
}

public class FacebookAuthRequest
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
} 