using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Models.Entities;

public class Booking
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PlayerId { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime Date { get; set; }
    public User? User { get; set; }
    public Player? Player { get; set; }
} 