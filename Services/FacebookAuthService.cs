using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FootballClub_Backend.Models;
using FootballClub_Backend.Exceptions;

namespace FootballClub_Backend.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<FacebookAuthService> _logger;

        public FacebookAuthService(
            HttpClient httpClient,
            IConfiguration configuration,
            ILogger<FacebookAuthService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<FacebookUserData> ValidateAccessTokenAsync(string accessToken)
        {
            try
            {
                var appId = _configuration["Authentication:Facebook:AppId"];
                var response = await _httpClient.GetAsync(
                    $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}&app_id={appId}");

                if (!response.IsSuccessStatusCode)
                {
                    throw new AuthenticationException("Invalid Facebook access token");
                }

                var content = await response.Content.ReadAsStringAsync();
                var userData = JsonSerializer.Deserialize<FacebookUserData>(content);

                if (userData == null)
                {
                    throw new AuthenticationException("Failed to deserialize Facebook user data");
                }

                return userData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating Facebook access token");
                throw;
            }
        }
    }
} 