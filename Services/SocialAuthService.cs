using Google.Apis.Auth;
using FootballClub_Backend.Models;
using System.Net.Http.Json;
using FootballClub_Backend.Exceptions;
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Helpers;

namespace FootballClub_Backend.Services;

public interface ISocialAuthService
{
    Task<User> AuthenticateGoogleUserAsync(string credential);
    Task<User> AuthenticateFacebookUserAsync(string accessToken);
}

public class SocialAuthService : ISocialAuthService
{
    private readonly IAuthService _authService;
    private readonly ILogger<SocialAuthService> _logger;

    public SocialAuthService(
        IAuthService authService,
        ILogger<SocialAuthService> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    public async Task<User> AuthenticateGoogleUserAsync(string credential)
    {
        try
        {
            var payload = await SocialAuthHelper.ValidateGoogleToken(credential);
            return await _authService.GetOrCreateGoogleUser(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google authentication failed");
            throw;
        }
    }

    public async Task<User> AuthenticateFacebookUserAsync(string accessToken)
    {
        try
        {
            var userData = await SocialAuthHelper.ValidateFacebookToken(accessToken);
            return await _authService.GetOrCreateFacebookUserAsync(userData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Facebook authentication failed");
            throw;
        }
    }
} 