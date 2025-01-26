using FootballClub_Backend.Models;

namespace FootballClub_Backend.Services;

public interface IPlayerService
{
    Task<IEnumerable<Player>> GetAllPlayers();
    Task<Player?> GetPlayerById(int id);
    Task<Player> CreatePlayer(Player player);
    Task<Player?> UpdatePlayer(int id, Player player);
    Task<bool> DeletePlayer(int id);
} 