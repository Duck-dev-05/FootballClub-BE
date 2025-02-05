using FootballClub_Backend.Models.Entities;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public interface ISocialAuthService
{
    Task<Models.Entities.User> AuthenticateGoogleUserAsync(string credential);
    Task<Models.Entities.User> AuthenticateFacebookUserAsync(string accessToken);
} 