using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Services;
using FootballClub_Backend.Data;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CalendarController : ApiController
{
    private readonly ILogger<CalendarController> _logger;
    private readonly ApplicationDbContext _context;

    public CalendarController(
        ILogger<CalendarController> logger, 
        ApplicationDbContext context)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
    {
        try
        {
            var events = await _context.CalendarEvents.ToListAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving calendar events");
            return HandleError(ex, "Failed to retrieve calendar events");
        }
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<CalendarEvent>> CreateEvent(CalendarEvent calendarEvent)
    {
        try
        {
            ValidateModel();
            calendarEvent.CreatedBy = GetUserId();
            _context.CalendarEvents.Add(calendarEvent);
            await _context.SaveChangesAsync();
            return HandleResult(calendarEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating calendar event");
            return HandleError(ex, "Failed to create calendar event");
        }
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<CalendarEvent>> UpdateEvent(int id, CalendarEvent calendarEvent)
    {
        try
        {
            var existingEvent = await _context.CalendarEvents.FindAsync(id);
            if (existingEvent == null)
                return NotFound(new { message = "Event not found" });

            if (existingEvent.CreatedBy != GetUserId() && GetUserRole() != "admin")
                return Unauthorized(new { message = "Not authorized to update this event" });

            _context.Entry(existingEvent).CurrentValues.SetValues(calendarEvent);
            await _context.SaveChangesAsync();
            return HandleResult(calendarEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating calendar event {Id}", id);
            return HandleError(ex, "Failed to update calendar event");
        }
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        try
        {
            var calendarEvent = await _context.CalendarEvents.FindAsync(id);
            if (calendarEvent == null)
                return NotFound(new { message = "Event not found" });

            if (calendarEvent.CreatedBy != GetUserId() && GetUserRole() != "admin")
                return Unauthorized(new { message = "Not authorized to delete this event" });

            _context.CalendarEvents.Remove(calendarEvent);
            await _context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting calendar event {Id}", id);
            return HandleError(ex, "Failed to delete calendar event");
        }
    }
} 