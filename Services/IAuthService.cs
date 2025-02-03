using System.Security.Claims;
using FootballClub_Backend.Models;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public interface IAuthService
{
    string GetRequesterRole(ClaimsPrincipal user);
    Task<(bool isSuccess, string token)> Register(RegisterRequest request, string requesterRole);
    Task<(bool isSuccess, string token)> Login(LoginRequest request);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
    Task<bool> UpdateUserRoleAsync(int userId, string role);
    string GenerateJwtToken(User user);
    Task<User> GetOrCreateFacebookUserAsync(FacebookUserData userData);
    Task<User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload);
} 