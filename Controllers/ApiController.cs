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

<<<<<<< HEAD
    protected ActionResult HandleError(Exception ex, string message = "An error occurred")
    {
        return ex switch
        {
            AuthenticationException _ => Unauthorized(new { message = ex.Message }),
            KeyNotFoundException _ => NotFound(new { message = ex.Message }),
            _ => StatusCode(500, new { message })
        };
=======
    protected ActionResult HandleError(Exception ex, string? customMessage = null)
    {
        if (ex is AuthenticationException)
            return Unauthorized(new { message = customMessage ?? ex.Message });
            
        return StatusCode(500, new { message = customMessage ?? "An internal server error occurred" });
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
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
<<<<<<< HEAD
        return User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "user";
=======
        var roleClaim = User.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? "user";
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
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