using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Requests;

public class BookingRequest
{
    [Required]
    public string FacilityId { get; set; } = string.Empty;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }
} 