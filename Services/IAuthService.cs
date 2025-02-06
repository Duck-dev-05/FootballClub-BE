using FootballClub_Backend.Models.Requests;
using FootballClub_Backend.Models.Responses;
using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<AuthResponse> AuthenticateGoogleUserAsync(string credential);
    Task<AuthResponse> AuthenticateFacebookUserAsync(string accessToken);
} 