using Google.Apis.Auth;
<<<<<<< HEAD
using FootballClub_Backend.Models.Entities;
using FootballClub_Backend.Models.Config;
using FootballClub_Backend.Models;
using FootballClub_Backend.Exceptions;
using FootballClub_Backend.Data;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace FootballClub_Backend.Services;

public class SocialAuthService : ISocialAuthService
{
    private readonly IAuthService _authService;
    private readonly AuthSettings _authSettings;
    private readonly ILogger<SocialAuthService> _logger;
    private readonly HttpClient _httpClient;
    private readonly MongoDbContext _context;

    public SocialAuthService(
        IAuthService authService,
        IOptions<AuthSettings> authSettings,
        ILogger<SocialAuthService> logger,
        HttpClient httpClient,
        MongoDbContext context)
    {
        _authService = authService;
        _authSettings = authSettings.Value;
        _logger = logger;
        _httpClient = httpClient;
        _context = context;
    }

    public async Task<UserEntity> AuthenticateGoogleUserAsync(string credential)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _authSettings.Google.ClientId }
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(credential, settings);
=======
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
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
            return await _authService.GetOrCreateGoogleUser(payload);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google authentication failed");
<<<<<<< HEAD
            throw new AuthenticationException("Failed to authenticate with Google", ex);
        }
    }

    public async Task<UserEntity> AuthenticateFacebookUserAsync(string accessToken)
    {
        try
        {
            var fbUser = await GetFacebookUserDataAsync(accessToken);
            return await _authService.GetOrCreateFacebookUserAsync(fbUser);
=======
            throw;
        }
    }

    public async Task<User> AuthenticateFacebookUserAsync(string accessToken)
    {
        try
        {
            var userData = await SocialAuthHelper.ValidateFacebookToken(accessToken);
            return await _authService.GetOrCreateFacebookUserAsync(userData);
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Facebook authentication failed");
<<<<<<< HEAD
            throw new AuthenticationException("Failed to authenticate with Facebook", ex);
        }
    }

    private async Task<FacebookUserData> GetFacebookUserDataAsync(string accessToken)
    {
        var fields = "id,name,email";
        var url = $"https://graph.facebook.com/v18.0/me?fields={fields}&access_token={accessToken}";
        
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw new AuthenticationException("Failed to get Facebook user data");
        }

        return await response.Content.ReadFromJsonAsync<FacebookUserData>() 
            ?? throw new AuthenticationException("Invalid Facebook user data");
    }
=======
            throw;
        }
    }
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
} 