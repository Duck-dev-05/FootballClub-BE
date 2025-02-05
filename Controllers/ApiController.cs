using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Linq;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected ActionResult<T> HandleResult<T>(T result)
    {
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    protected ActionResult HandleError(Exception ex, string? customMessage = null)
    {
        if (ex is AuthenticationException)
            return Unauthorized(new { message = customMessage ?? ex.Message });
            
        return StatusCode(500, new { message = customMessage ?? "An internal server error occurred" });
    }

    protected int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            throw new AuthenticationException("User ID not found in token");
            
        return userId;
    }

    protected string GetUserRole()
    {
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? "user";
    }

    protected void ValidateModel()
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            throw new ValidationException(string.Join(", ", errors));
        }
    }
} 