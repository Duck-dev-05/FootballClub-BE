using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using FootballClub_Backend.Models;

namespace FootballClub_Backend.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public FacebookAuthService(
            HttpClient httpClient,
            IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<(bool IsValid, FacebookUserData? UserData)> ValidateTokenAsync(string accessToken)
        {
            try
            {
                var response = await _httpClient.GetAsync(
                    $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}");

                if (!response.IsSuccessStatusCode)
                    return (false, null);

                var userData = await response.Content.ReadFromJsonAsync<FacebookUserData>();
                return (true, userData);
            }
            catch
            {
                return (false, null);
            }
        }
    }
} 