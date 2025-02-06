using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Entities;

public class Gallery
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
} 