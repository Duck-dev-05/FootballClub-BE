namespace FootballClub_Backend.Models.Entities;

public class Ticket
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int MatchId { get; set; }
    public decimal Price { get; set; }
} 