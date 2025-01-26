using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System.IO;
using FootballClub_Backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace FootballClub_Backend.Helpers;

public static class SocialAuthHelper
{
    private static readonly IConfiguration _configuration;

    static SocialAuthHelper()
    {
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();
    }

    public static async Task<ActionResult<GoogleJsonWebSignature.Payload>> ValidateGoogleToken(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            };
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            if (payload == null)
                return new BadRequestObjectResult("Invalid Google token");
            
            return new OkObjectResult(payload);
        }
        catch (Exception)
        {
            return new BadRequestObjectResult("Failed to validate Google token");
        }
    }

    public static async Task<ActionResult<FacebookUserData>> ValidateFacebookToken(string accessToken)
    {
        var appId = _configuration["Authentication:Facebook:AppId"];
        var client = new HttpClient();
        var response = await client.GetAsync(
            $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}&app_id={appId}");
        
        if (!response.IsSuccessStatusCode)
            return new BadRequestObjectResult("Invalid Facebook token");

        var content = await response.Content.ReadAsStringAsync();
        var userData = JsonSerializer.Deserialize<FacebookUserData>(content);
        if (userData == null)
            return new BadRequestObjectResult("Failed to deserialize user data");
        
        return new OkObjectResult(userData);
    }
} 