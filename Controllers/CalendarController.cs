using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Data;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CalendarController : ApiController
{
    private readonly ILogger<CalendarController> _logger;
    private readonly MongoDbContext _context;

    public CalendarController(
        ILogger<CalendarController> logger, 
        MongoDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CalendarEvent>>> GetEvents()
    {
        try
        {
            var events = await _context.CalendarEvents.Find(_ => true).ToListAsync();
            return Ok(events);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving calendar events");
            return HandleError(ex, "Failed to retrieve calendar events");
        }
    }

    [HttpPost]
    public async Task<ActionResult<CalendarEvent>> CreateEvent(CalendarEvent calendarEvent)
    {
        try
        {
            await _context.CalendarEvents.InsertOneAsync(calendarEvent);
            return CreatedAtAction(nameof(GetEvents), new { id = calendarEvent.Id }, calendarEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating calendar event");
            return HandleError(ex, "Failed to create calendar event");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CalendarEvent>> UpdateEvent(string id, CalendarEvent calendarEvent)
    {
        try
        {
            var result = await _context.CalendarEvents.ReplaceOneAsync(
                e => e.Id == id,
                calendarEvent);

            if (result.ModifiedCount == 0)
                return NotFound();

            return Ok(calendarEvent);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating calendar event");
            return HandleError(ex, "Failed to update calendar event");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEvent(string id)
    {
        try
        {
            var result = await _context.CalendarEvents.DeleteOneAsync(e => e.Id == id);
            
            if (result.DeletedCount == 0)
                return NotFound();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting calendar event");
            return HandleError(ex, "Failed to delete calendar event");
        }
    }
} 