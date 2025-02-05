using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Requests;

public class FacebookAuthRequest
{
    [Required]
    public string AccessToken { get; set; } = string.Empty;
} 