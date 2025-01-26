using FootballClub_Backend.Data;
using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Collections.Generic;
using System;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new KeyNotFoundException($"User with email {email} not found");
        return user;
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException($"User with ID {id} not found");
        return user;
    }

    public string GetRequesterRole(ClaimsPrincipal user)
    {
        if (!user.Identity?.IsAuthenticated ?? true)
            return "user";

        var userRole = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
        return userRole == "admin" ? "admin" : "user";
    }

    public async Task<(bool, string)> Register(RegisterRequest request, string requesterRole = "user")
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            return (false, "User already exists.");

        string role = "user";
        if (requesterRole == "admin" && !string.IsNullOrEmpty(request.Role))
        {
            role = request.Role;
        }

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = role
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateToken(user);
        return (true, token);
    }

    public async Task<(bool, string)> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return (false, "Invalid credentials.");

        var token = GenerateToken(user);
        return (true, token);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            // Log the role change
            var auditLog = new UserAuditLog
            {
                UserId = userId,
                Action = "ROLE_CHANGE",
                PreviousState = System.Text.Json.JsonSerializer.Serialize(user),
                Timestamp = DateTime.UtcNow
            };
            _context.UserAuditLogs.Add(auditLog);

            user.Role = newRole;
            _context.Users.Update(user);
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

    public string GenerateToken(User user)
    {
        var jwtKey = _configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT Token key is not configured");
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
        if (user != null) return user;

        user = new User
        {
            Email = payload.Email,
            Username = payload.Name,
            Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Random password
            Role = "user",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetOrCreateFacebookUser(FacebookUserData userData)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
        if (user != null) return user;

        user = new User
        {
            Email = userData.Email,
            Username = userData.Name,
            Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Random password
            Role = "user",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
} 