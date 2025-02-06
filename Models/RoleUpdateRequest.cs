using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models;

public class RoleUpdateRequest
{
    [Required]
    public string Role { get; set; } = string.Empty;
} 