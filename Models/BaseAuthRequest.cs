using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models;

public abstract class BaseAuthRequest
{
    [Required]
    public string Token { get; set; } = string.Empty;
} 