<<<<<<< HEAD
 
=======
using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models;

public class User
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
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public string? FacebookId { get; set; }
    
    public string? GoogleId { get; set; }
}

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
} 
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
