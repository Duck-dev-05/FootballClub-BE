using FootballClub_Backend.Models.Entities;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public interface ISocialAuthService
{
    Task<UserEntity> AuthenticateGoogleUserAsync(string credential);
    Task<UserEntity> AuthenticateFacebookUserAsync(string accessToken);
} 