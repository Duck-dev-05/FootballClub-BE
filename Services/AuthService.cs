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
using BCrypt.Net;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Models.Requests;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        ApplicationDbContext context,
        IJwtService jwtService,
        ILogger<AuthService> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    public string GetRequesterRole(ClaimsPrincipal user)
    {
        var roleClaim = user.FindFirst(ClaimTypes.Role);
        return roleClaim?.Value ?? "user";
    }

    public async Task<(bool success, string token)> Register(Models.Requests.RegisterRequest request, string requesterRole)
    {
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (existingUser != null)
            return (success: false, token: string.Empty);

        var user = new Models.Entities.User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            Role = requesterRole == "admin" ? request.Role ?? "user" : "user"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        return (success: true, token: token);
    }

    public async Task<(bool success, string token)> Login(Models.Requests.LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            return (success: false, token: string.Empty);

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return (success: false, token: string.Empty);

        var token = GenerateJwtToken(user);
        return (success: true, token: token);
    }

    public string GenerateJwtToken(Models.Entities.User user)
    {
        return _jwtService.GenerateToken(user);
    }

    public async Task<Models.Entities.User?> GetUserByEmail(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<Models.Entities.User?> GetUserById(int id)
    {
        return await _context.Users.FindAsync(id);
    }

    public async Task<IEnumerable<Models.Entities.User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserRoleAsync(int userId, string newRole)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.Role = newRole;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<Models.Entities.User> GetOrCreateFacebookUserAsync(FacebookUserData userData)
    {
        var user = await GetUserByEmail(userData.Email);
        if (user != null) return user;

        user = new Models.Entities.User
        {
            Email = userData.Email,
            Username = userData.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
            Role = "user"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<Models.Entities.User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload)
    {
        var user = await GetUserByEmail(payload.Email);
        if (user != null) return user;

        user = new Models.Entities.User
        {
            Email = payload.Email,
            Username = payload.Name,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()),
            Role = "user"
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }
} 