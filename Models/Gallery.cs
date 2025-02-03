namespace FootballClub_Backend.Models;

public class Gallery
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime UploadDate { get; set; } = DateTime.UtcNow;
    public string Category { get; set; } = "General"; // Match, Training, Event, etc.
} 