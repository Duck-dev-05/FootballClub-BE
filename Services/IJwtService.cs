using FootballClub_Backend.Models.Entities;

namespace FootballClub_Backend.Services;

public interface IJwtService
{
    string GenerateToken(User user);
} 