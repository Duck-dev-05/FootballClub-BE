using FootballClub_Backend.Data;
using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace FootballClub_Backend.Services;

public interface IUserService
{
    Task<ActionResult<UserDto>> GetUserProfile(int userId);
    Task<bool> DeleteAccount(int userId);
    Task<bool> RollbackUserChanges(int userId);
    Task<List<UserAuditLog>> GetUserAuditLogs(int userId);
}

public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;

    public UserService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActionResult<UserDto>> GetUserProfile(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return new NotFoundResult();

        return new OkObjectResult(new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role
        });
    }

    public async Task<bool> DeleteAccount(int userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            // Log the deletion
            var auditLog = new UserAuditLog
            {
                UserId = userId,
                Action = "DELETE_ACCOUNT",
                PreviousState = System.Text.Json.JsonSerializer.Serialize(user),
                Timestamp = DateTime.UtcNow
            };
            _context.UserAuditLogs.Add(auditLog);

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> RollbackUserChanges(int userId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var lastAuditLog = await _context.UserAuditLogs
                .Where(l => l.UserId == userId)
                .OrderByDescending(l => l.Timestamp)
                .FirstOrDefaultAsync();

            if (lastAuditLog == null) return false;

            var previousState = System.Text.Json.JsonSerializer.Deserialize<User>(lastAuditLog.PreviousState);
            if (previousState == null) return false;

            var currentUser = await _context.Users.FindAsync(userId);

            if (currentUser == null && lastAuditLog.Action == "DELETE_ACCOUNT")
            {
                // Restore deleted user
                previousState.Id = userId; // Ensure the ID is set correctly
                _context.Users.Add(previousState);
            }
            else if (currentUser != null)
            {
                // Restore previous state
                currentUser.Username = previousState.Username;
                currentUser.Email = previousState.Email;
                currentUser.Role = previousState.Role;
                _context.Users.Update(currentUser);
            }

            // Remove the audit log entry
            _context.UserAuditLogs.Remove(lastAuditLog);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<List<UserAuditLog>> GetUserAuditLogs(int userId)
    {
        return await _context.UserAuditLogs
            .Where(l => l.UserId == userId)
            .OrderByDescending(l => l.Timestamp)
            .ToListAsync();
    }
} 