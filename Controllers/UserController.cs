using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using System.Security.Claims;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _userService.GetUserProfile(userId);
        return result;
    }

    [HttpDelete("account")]
    public async Task<IActionResult> DeleteAccount()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var result = await _userService.DeleteAccount(userId);
        if (!result)
            return NotFound();

        return NoContent();
    }

    [HttpPost("rollback")]
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> RollbackChanges(int userId)
    {
        var result = await _userService.RollbackUserChanges(userId);
        if (!result)
            return NotFound("No changes to rollback");

        return Ok("Changes rolled back successfully");
    }

    [HttpGet("audit-logs")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<List<UserAuditLog>>> GetAuditLogs(int userId)
    {
        var logs = await _userService.GetUserAuditLogs(userId);
        return Ok(logs);
    }
} 