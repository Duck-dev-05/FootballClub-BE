using FootballClub_Backend.Data;
using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballClub_Backend.Services;

public class BookingService : IBookingService
{
    private readonly ApplicationDbContext _context;

    public BookingService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Booking>> GetAllBookings()
    {
        return await _context.Bookings.ToListAsync();
    }

    public async Task<Booking?> GetBookingById(int id)
    {
        return await _context.Bookings.FindAsync(id);
    }

    public async Task<IEnumerable<string>> GetAvailableSlots(int playerId, DateTime date)
    {
        var bookedSlots = await _context.Bookings
            .Where(b => b.PlayerId == playerId && b.Date.Date == date.Date)
            .Select(b => b.TimeSlot)
            .ToListAsync();

        var allSlots = new[]
        {
            "09:00", "10:00", "11:00", "13:00", "14:00", "15:00", "16:00"
        };

        return allSlots.Except(bookedSlots);
    }

    public async Task<Booking> CreateBooking(BookingRequest request)
    {
        var booking = new Booking
        {
            PlayerId = request.PlayerId,
            UserId = request.UserId,
            Date = request.Date,
            TimeSlot = request.TimeSlot
        };

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        return booking;
    }

    public async Task<bool> DeleteBooking(int id)
    {
        var booking = await _context.Bookings.FindAsync(id);
        if (booking == null)
            return false;

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        return true;
    }
} 