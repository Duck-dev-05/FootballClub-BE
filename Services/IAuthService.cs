using System.Security.Claims;
using FootballClub_Backend.Models;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Models.Requests;
using FootballClub_Backend.Models.Responses;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public interface IAuthService
{
    string GetRequesterRole(ClaimsPrincipal user);
    Task<(bool success, string token)> Register(RegisterRequest request, string requesterRole);
    Task<(bool success, string token)> Login(LoginRequest request);
    Task<Models.Entities.User?> GetUserByEmail(string email);
    Task<Models.Entities.User?> GetUserById(int id);
    Task<IEnumerable<Models.Entities.User>> GetAllUsersAsync();
    Task<bool> UpdateUserRoleAsync(int userId, string newRole);
    string GenerateJwtToken(Models.Entities.User user);
    Task<Models.Entities.User> GetOrCreateFacebookUserAsync(FacebookUserData userData);
    Task<Models.Entities.User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload);
} 