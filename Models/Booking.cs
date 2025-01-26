namespace FootballClub_Backend.Models;

public class Booking
{
    public int Id { get; set; }
    public int PlayerId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
}

public class BookingRequest
{
    public int PlayerId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
} 