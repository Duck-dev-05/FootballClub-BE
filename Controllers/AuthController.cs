using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using FootballClub_Backend.Helpers;
using Google.Apis.Auth;
using System.ComponentModel.DataAnnotations;
using FootballClub_Backend.Exceptions;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using FootballClub_Backend.Models.Requests;
using FootballClub_Backend.Models.Responses;
using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ApiController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IFacebookAuthService _facebookAuthService;
    private readonly IConfiguration _configuration;
    private readonly ISocialAuthService _socialAuthService;

    public AuthController(
        IAuthService authService,
        ILogger<AuthController> logger,
        IFacebookAuthService facebookAuthService,
        IConfiguration configuration,
        ISocialAuthService socialAuthService)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _facebookAuthService = facebookAuthService ?? throw new ArgumentNullException(nameof(facebookAuthService));
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        _socialAuthService = socialAuthService ?? throw new ArgumentNullException(nameof(socialAuthService));
    }

    [HttpPost("register")]
    public async Task<ActionResult<Models.Responses.AuthResponse>> Register(Models.Requests.RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid registration data" });

            string requesterRole = GetUserRole();
            var (success, token) = await _authService.Register(request, requesterRole);
            
            if (!success)
                return BadRequest(new { message = "Registration failed" });

            var user = await _authService.GetUserByEmail(request.Email);
            return HandleResult(new Models.Responses.AuthResponse 
            { 
                Token = token,
                User = new Models.Responses.UserDto(user)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Registration failed for {Email}", request.Email);
            return HandleError(ex, "Registration failed");
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<Models.Responses.AuthResponse>> Login(Models.Requests.LoginRequest request)
    {
        try
        {
            var (success, token) = await _authService.Login(request);
            if (!success)
                return Unauthorized(new { message = "Invalid credentials" });

            var user = await _authService.GetUserByEmail(request.Email);
            return HandleResult(new Models.Responses.AuthResponse 
            { 
                Token = token,
                User = new Models.Responses.UserDto(user)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for {Email}", request.Email);
            return HandleError(ex, "Login failed");
        }
    }

    [HttpGet("users")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<IEnumerable<Models.Responses.UserDto>>> GetUsers()
    {
        try
        {
            var users = await _authService.GetAllUsersAsync();
            return HandleResult(users.Select(u => new Models.Responses.UserDto(u)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users list");
            return HandleError(ex, "Failed to retrieve users");
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("users/{id}/role")]
    public async Task<ActionResult<Models.Responses.UserDto>> UpdateUserRole(int id, Models.Requests.RoleUpdateRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new { message = "Invalid role data" });

            var success = await _authService.UpdateUserRoleAsync(id, request.Role);
            if (!success)
                return NotFound(new { message = "User not found" });

            var updatedUser = await _authService.GetUserById(id);
            return HandleResult(new Models.Responses.UserDto(updatedUser));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role for user {UserId}", id);
            return HandleError(ex, "Role update failed");
        }
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