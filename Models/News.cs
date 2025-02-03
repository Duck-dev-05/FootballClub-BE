namespace FootballClub_Backend.Models;

public class News
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
    public string Author { get; set; } = string.Empty;
    public bool IsPublished { get; set; } = true;
} 