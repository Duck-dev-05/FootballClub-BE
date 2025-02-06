using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
<<<<<<< HEAD
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using FootballClub_Backend.Models.Config;
=======
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Services;

public class JwtService : IJwtService
{
<<<<<<< HEAD
    private readonly AuthSettings _authSettings;

    public JwtService(IOptions<AuthSettings> authSettings)
    {
        _authSettings = authSettings.Value;
    }

    public string GenerateToken(UserEntity user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_authSettings.Jwt.Key);

=======
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Secret"]!);
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
<<<<<<< HEAD
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(_authSettings.Jwt.ExpiryInDays),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256Signature),
            Issuer = _authSettings.Jwt.Issuer,
            Audience = _authSettings.Jwt.Audience
=======
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            }),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
>>>>>>> 2fb1476a74c18a73f96b820f9c5b95143924086b
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
} 