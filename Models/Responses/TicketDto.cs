namespace FootballClub_Backend.Models.Responses;

public class TicketDto
{
    public string Id { get; set; } = string.Empty;
    public string MatchId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string Section { get; set; } = string.Empty;
    public string Seat { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public TicketDto() { }

    public TicketDto(Entities.Ticket ticket)
    {
        Id = ticket.Id;
        MatchId = ticket.MatchId;
        UserId = ticket.UserId;
        Section = ticket.Section;
        Seat = ticket.Seat;
        Price = ticket.Price;
        Status = ticket.Status;
        CreatedAt = ticket.CreatedAt;
    }
} 