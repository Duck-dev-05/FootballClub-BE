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
using FootballClub_Backend.Exceptions;

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

    public static async Task<GoogleJsonWebSignature.Payload> ValidateGoogleToken(string token)
    {
        try
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            };
            
            var payload = await GoogleJsonWebSignature.ValidateAsync(token, settings);
            if (payload == null)
                throw new AuthenticationException("Invalid Google token");
            
            return payload;
        }
        catch (Exception)
        {
            throw new AuthenticationException("Failed to validate Google token");
        }
    }

    public static async Task<FacebookUserData> ValidateFacebookToken(string accessToken)
    {
        var appId = _configuration["Authentication:Facebook:AppId"];
        using var client = new HttpClient();
        var response = await client.GetAsync(
            $"https://graph.facebook.com/me?fields=id,name,email&access_token={accessToken}&app_id={appId}");
        
        if (!response.IsSuccessStatusCode)
            throw new AuthenticationException("Invalid Facebook token");

        var content = await response.Content.ReadAsStringAsync();
        var userData = JsonSerializer.Deserialize<FacebookUserData>(content);
        if (userData == null)
            throw new AuthenticationException("Failed to deserialize user data");
        
        return userData;
    }
} 