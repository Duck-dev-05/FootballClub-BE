using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models;

public class FacebookAuthRequest
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
} 