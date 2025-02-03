using FootballClub_Backend.Models;

namespace FootballClub_Backend.Services
{
    public interface IFacebookAuthService
    {
        Task<(bool IsValid, FacebookUserData? UserData)> ValidateTokenAsync(string accessToken);
    }
} 