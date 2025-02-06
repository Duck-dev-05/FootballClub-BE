using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using FootballClub_Backend.Models.Requests;
using FootballClub_Backend.Models.Responses;

namespace FootballClub_Backend.Controllers;

[Route("api/auth/social")]
[ApiController]
public class SocialAuthController : ApiController
{
    private readonly ISocialAuthService _socialAuthService;
    private readonly IAuthService _authService;
    private readonly ILogger<SocialAuthController> _logger;

    public SocialAuthController(
        ISocialAuthService socialAuthService,
        IAuthService authService,
        ILogger<SocialAuthController> logger)
    {
        _socialAuthService = socialAuthService;
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("google")]
    public async Task<ActionResult<Models.Responses.AuthResponse>> GoogleLogin([FromBody] Models.Requests.GoogleAuthRequest request)
    {
        try
        {
            var user = await _socialAuthService.AuthenticateGoogleUserAsync(request.Credential);
            var token = _authService.GenerateJwtToken(user);

            return HandleResult(new Models.Responses.AuthResponse
            {
                Token = token,
                User = new Models.Responses.UserDto(user)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google authentication failed");
            return HandleError(ex);
        }
    }

    [HttpPost("facebook")]
    public async Task<ActionResult<Models.Responses.AuthResponse>> FacebookLogin([FromBody] Models.Requests.FacebookAuthRequest request)
    {
        try
        {
            var user = await _socialAuthService.AuthenticateFacebookUserAsync(request.AccessToken);
            var token = _authService.GenerateJwtToken(user);

            return HandleResult(new Models.Responses.AuthResponse
            {
                Token = token,
                User = new Models.Responses.UserDto(user)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Facebook authentication failed");
            return HandleError(ex);
        }
    }
} 