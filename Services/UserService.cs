using FootballClub_Backend.Data;
using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballClub_Backend.Services;

public interface IUserService
{
    Task<UserDto> GetUserProfile(int userId);
    Task<bool> DeleteAccount(int userId);
    Task<bool> RollbackUserChanges(int userId);
    Task<List<UserAuditLog>> GetUserAuditLogs(int userId);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<UserDto> GetUserProfile(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            throw new KeyNotFoundException("User not found");

        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        };
    }

    public async Task<bool> DeleteAccount(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        // Log the deletion
        var auditLog = new UserAuditLog
        {
            UserId = userId,
            Action = "DELETE_ACCOUNT",
            Details = $"Account deleted for user {user.Email}"
        };
        _context.UserAuditLogs.Add(auditLog);

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RollbackUserChanges(int userId)
    {
        var lastLog = await _context.UserAuditLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .FirstOrDefaultAsync();

        if (lastLog == null)
            return false;

        // Log the rollback
        var rollbackLog = new UserAuditLog
        {
            UserId = userId,
            Action = "ROLLBACK",
            Details = $"Rolled back action: {lastLog.Action}"
        };
        _context.UserAuditLogs.Add(rollbackLog);

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<List<UserAuditLog>> GetUserAuditLogs(int userId)
    {
        return await _context.UserAuditLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }
} 