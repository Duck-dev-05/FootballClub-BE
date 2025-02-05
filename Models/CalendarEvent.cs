using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models;

public class CalendarEvent
{
    public int Id { get; set; }

    [Required]
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    [Required]
    public DateTime StartTime { get; set; }

    [Required]
    public DateTime EndTime { get; set; }

    public int CreatedBy { get; set; }

    public User? Creator { get; set; }
} 