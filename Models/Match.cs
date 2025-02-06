namespace FootballClub_Backend.Models;

public class Match
{
    public int Id { get; set; }
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public DateTime MatchDate { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string Competition { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public int AvailableTickets { get; set; }
    public bool IsSoldOut { get; set; }
} 