using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BookingController : ControllerBase
{
    private readonly IBookingService _bookingService;

    public BookingController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllBookings();
        return Ok(bookings);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Booking>> GetBooking(int id)
    {
        var booking = await _bookingService.GetBookingById(id);
        if (booking == null)
            return NotFound();

        return Ok(booking);
    }

    [HttpGet("available-slots")]
    public async Task<ActionResult<IEnumerable<string>>> GetAvailableSlots([FromQuery] int playerId, [FromQuery] DateTime date)
    {
        var slots = await _bookingService.GetAvailableSlots(playerId, date);
        return Ok(slots);
    }

    [HttpPost]
    public async Task<ActionResult<Booking>> CreateBooking(BookingRequest request)
    {
        var booking = await _bookingService.CreateBooking(request);
        return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteBooking(int id)
    {
        var result = await _bookingService.DeleteBooking(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 