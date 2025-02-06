namespace FootballClub_Backend.Models;

public class Ticket
{
    public int Id { get; set; }
    public int MatchId { get; set; }
    public int UserId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Active"; // Active, Used, Cancelled
    public string PaymentStatus { get; set; } = "Pending"; // Pending, Completed, Failed
    public string? PaymentId { get; set; }
} 