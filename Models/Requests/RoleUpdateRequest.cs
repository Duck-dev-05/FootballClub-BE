using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Requests;

public class RoleUpdateRequest
{
    [Required]
    public string Role { get; set; } = string.Empty;
} 