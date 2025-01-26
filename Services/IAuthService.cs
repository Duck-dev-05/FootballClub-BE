using FootballClub_Backend.Models;
using Google.Apis.Auth;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Security.Claims;

namespace FootballClub_Backend.Services;

public interface IAuthService
{
    Task<(bool success, string token)> Register(RegisterRequest request, string requesterRole = "user");
    Task<(bool success, string token)> Login(LoginRequest request);
    Task<User> GetUserByEmail(string email);
    Task<User> GetUserById(int id);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> UpdateUserRoleAsync(int userId, string newRole);
    string GenerateToken(User user);
    Task<User> GetOrCreateGoogleUser(GoogleJsonWebSignature.Payload payload);
    Task<User> GetOrCreateFacebookUser(FacebookUserData userData);
    string GetRequesterRole(ClaimsPrincipal user);
} 