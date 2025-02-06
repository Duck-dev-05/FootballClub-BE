using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models
{
    public class LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;
    }

    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        public string? Role { get; set; }
    }

    public class GoogleAuthRequest
    {
        [Required]
        public required string Credential { get; set; }
    }

    public class FacebookAuthRequest
    {
        [Required]
        public required string AccessToken { get; set; }
    }

    public class RoleUpdateRequest
    {
        [Required]
        public string Role { get; set; } = string.Empty;
    }

    public class AuthResponse
    {
        public string Token { get; set; } = string.Empty;
        public UserDto User { get; set; } = null!;
    }

    public class ErrorResponse
    {
        public string Type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;

        public ErrorResponse(string type, string message)
        {
            Type = type;
            Message = message;
        }
    }
} 