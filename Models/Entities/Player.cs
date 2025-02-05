using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Entities;

public class Player
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool IsAvailable { get; set; } = true;
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
} 