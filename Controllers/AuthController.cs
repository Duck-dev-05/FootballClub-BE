using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Linq;
using FootballClub_Backend.Helpers;
using Google.Apis.Auth;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
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
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid login credentials"));

            var (success, token) = await _authService.Login(request);
            if (!success)
                return Unauthorized(new ErrorResponse("Authentication Failed", "Invalid credentials"));

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
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "User not found: {Email}", request.Email);
            return NotFound(new ErrorResponse("Not Found", ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during login for user {Email}", request.Email);
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Login process failed"));
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
    public async Task<ActionResult<AuthResponse>> GoogleLogin(GoogleAuthRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Google credentials"));

            var result = await SocialAuthHelper.ValidateGoogleToken(request.Credential);
            if (result?.Result is not OkObjectResult okResult || okResult.Value == null)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Google token"));

            var payload = okResult.Value as GoogleJsonWebSignature.Payload;
            if (payload == null)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Google payload"));

            var user = await _authService.GetOrCreateGoogleUser(payload);
            var token = _authService.GenerateToken(user);

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
            _logger.LogError(ex, "Error during Google authentication");
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Google authentication failed"));
        }
    }

    [HttpPost("facebook")]
    public async Task<ActionResult<AuthResponse>> FacebookLogin(FacebookAuthRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Facebook credentials"));

            var result = await SocialAuthHelper.ValidateFacebookToken(request.AccessToken);
            if (result?.Result is not OkObjectResult okResult || okResult.Value == null)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Facebook token"));

            var userData = okResult.Value as FacebookUserData;
            if (userData == null)
                return BadRequest(new ErrorResponse("Validation Error", "Invalid Facebook user data"));

            var user = await _authService.GetOrCreateFacebookUser(userData);
            var token = _authService.GenerateToken(user);

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
            _logger.LogError(ex, "Error during Facebook authentication");
            return StatusCode(500, new ErrorResponse("Internal Server Error", "Facebook authentication failed"));
        }
    }
} 