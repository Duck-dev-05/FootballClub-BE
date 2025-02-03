using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using FootballClub_Backend.Helpers;
using Google.Apis.Auth;
using System.ComponentModel.DataAnnotations;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly IFacebookAuthService _facebookAuthService;
    private readonly IConfiguration _configuration;

    public AuthController(IAuthService authService, ILogger<AuthController> logger, IFacebookAuthService facebookAuthService, IConfiguration configuration)
    {
        _authService = authService;
        _logger = logger;
        _facebookAuthService = facebookAuthService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register(RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid registration data"));

            string requesterRole = _authService.GetRequesterRole(User);
            var (success, token) = await _authService.Register(request, requesterRole);
            
            if (!success)
                return BadRequest(new ErrorResponse("Registration Failed", token));

            return Ok(new AuthResponse 
            { 
                Token = token,
                User = new UserDto
                {
                    Username = request.Username,
                    Email = request.Email,
                    Role = requesterRole
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during registration for user {Email}", request.Email);
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Registration process failed"));
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login(LoginRequest request)
    {
        try
        {
            var (success, token) = await _authService.Login(request);
            if (!success)
                return Unauthorized(new { message = "Invalid credentials" });

            var user = await _authService.GetUserByEmail(request.Email);
            return Ok(new AuthResponse 
            { 
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user {Email}", request.Email);
            return StatusCode(500, new { message = "An error occurred during login" });
        }
    }

    [Authorize(Roles = "admin")]
    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        try
        {
            var users = await _authService.GetAllUsersAsync();
            return Ok(users.Select(u => new UserDto
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                Role = u.Role
            }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving users list");
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Failed to retrieve users"));
        }
    }

    [Authorize(Roles = "admin")]
    [HttpPut("users/{id}/role")]
    public async Task<ActionResult<UserDto>> UpdateUserRole(int id, RoleUpdateRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid role data"));

            var success = await _authService.UpdateUserRoleAsync(id, request.Role);
            if (!success)
                return NotFound(new ErrorResponse("Not Found", "User not found"));

            var updatedUser = await _authService.GetUserById(id);
            return Ok(new UserDto
            {
                Id = updatedUser.Id,
                Username = updatedUser.Username,
                Email = updatedUser.Email,
                Role = updatedUser.Role
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating role for user {UserId}", id);
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Role update failed"));
        }
    }

    [HttpPost("google")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponse>> GoogleLogin([FromBody] GoogleAuthRequest request)
    {
        try
        {
            _logger.LogInformation("Starting Google authentication process");

            if (string.IsNullOrEmpty(request.Credential))
            {
                _logger.LogWarning("Google credential is missing");
                return BadRequest(new { message = "Google credential is required" });
            }

            var settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _configuration["Authentication:Google:ClientId"] }
            };

            _logger.LogInformation("Validating Google token");
            var payload = await GoogleJsonWebSignature.ValidateAsync(request.Credential, settings);
            
            _logger.LogInformation("Google token validated successfully for email: {Email}", payload.Email);

            var user = await _authService.GetOrCreateGoogleUser(payload);
            var token = _authService.GenerateJwtToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
        catch (InvalidJwtException ex)
        {
            _logger.LogError(ex, "Invalid Google token");
            return BadRequest(new { message = "Invalid Google token" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Google authentication failed: {Message}", ex.Message);
            return StatusCode(500, new { message = "An error occurred during Google authentication" });
        }
    }

    [HttpPost("facebook")]
    public async Task<ActionResult<AuthResponse>> FacebookLogin([FromBody] FacebookAuthRequest request)
    {
        try
        {
            var fbValidation = await _facebookAuthService.ValidateTokenAsync(request.AccessToken);
            if (!fbValidation.IsValid || fbValidation.UserData == null)
                return Unauthorized(new { message = "Invalid Facebook token" });

            var user = await _authService.GetOrCreateFacebookUserAsync(fbValidation.UserData);
            var token = _authService.GenerateJwtToken(user);

            return Ok(new AuthResponse
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Facebook authentication failed");
            return StatusCode(500, new { message = "An error occurred during Facebook authentication" });
        }
    }
}

public class GoogleAuthRequest
{
    [Required]
    public string Credential { get; set; } = string.Empty;
} 