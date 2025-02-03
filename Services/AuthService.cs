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

namespace FootballClub_Backend.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public string GetRequesterRole(ClaimsPrincipal user)
    {
        return user.IsInRole("admin") ? "admin" : "user";
    }

    public async Task<(bool isSuccess, string token)> Register(RegisterRequest request, string requesterRole)
    {
        if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            return (false, "Email already registered");

        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);
        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = hashedPassword,
            Role = requesterRole == "admin" && request.Role != null ? request.Role : "user",
            CreatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return (true, GenerateJwtToken(user));
    }

    public async Task<(bool isSuccess, string token)> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (user == null)
            return (false, "Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return (false, "Invalid credentials");

        return (true, GenerateJwtToken(user));
    }

    public async Task<User> GetUserByEmail(string email)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null)
            throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
            throw new KeyNotFoundException("User not found");
        return user;
    }

    public async Task<IEnumerable<User>> GetAllUsersAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserRoleAsync(int userId, string role)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null)
            return false;

        user.Role = role;
        await _context.SaveChangesAsync();
        return true;
    }

    public string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JWT");
        var key = Encoding.ASCII.GetBytes(jwtSettings["Key"] ?? throw new InvalidOperationException("JWT Key is not configured"));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddHours(24),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"],
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task<User> GetOrCreateFacebookUserAsync(FacebookUserData userData)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userData.Email);
        if (user != null)
            return user;

        user = new User
        {
            Username = userData.Name,
            Email = userData.Email,
            Password = string.Empty, // Facebook users don't need a password
            Role = "user",
            CreatedAt = DateTime.UtcNow,
            FacebookId = userData.Id
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == payload.Email);
        
        if (user == null)
        {
            // Create new user
            user = new User
            {
                Username = payload.Name,
                Email = payload.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(Guid.NewGuid().ToString()), // Random password
                Role = "user",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Created new user from Google login: {Email}", payload.Email);
        }
        else
        {
            _logger.LogInformation("Existing user logged in via Google: {Email}", payload.Email);
        }

        return user;
    }
} 