using FootballClub_Backend.Models;

namespace FootballClub_Backend.Services
{
    public interface IFacebookAuthService
    {
        Task<FacebookUserData> ValidateAccessTokenAsync(string accessToken);
    }
} 