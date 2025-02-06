using FootballClub_Backend.Models.Entities;
using Google.Apis.Auth;

namespace FootballClub_Backend.Services;

public interface ISocialAuthService
{
<<<<<<< HEAD
    Task<UserEntity> AuthenticateGoogleUserAsync(string credential);
    Task<UserEntity> AuthenticateFacebookUserAsync(string accessToken);
=======
    Task<Models.Entities.User> AuthenticateGoogleUserAsync(string credential);
    Task<Models.Entities.User> AuthenticateFacebookUserAsync(string accessToken);
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 