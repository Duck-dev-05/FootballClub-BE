using FootballClub_Backend.Models;

namespace FootballClub_Backend.Services;

public interface IBookingService
{
    Task<IEnumerable<Booking>> GetAllBookings();
    Task<Booking?> GetBookingById(int id);
    Task<IEnumerable<string>> GetAvailableSlots(int playerId, DateTime date);
    Task<Booking> CreateBooking(BookingRequest request);
    Task<bool> DeleteBooking(int id);
} 