using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Requests;

public class GoogleAuthRequest
{
    [Required]
    public string Credential { get; set; } = string.Empty;
} 