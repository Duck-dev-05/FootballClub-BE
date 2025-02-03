using FootballClub_Backend.Data;
using FootballClub_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace FootballClub_Backend.Services;

public class PlayerService : IPlayerService
{
    private readonly ApplicationDbContext _context;

    public PlayerService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetAllPlayers()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player?> GetPlayerById(int id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task<Player> CreatePlayer(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> UpdatePlayer(int id, Player player)
    {
        var existingPlayer = await _context.Players.FindAsync(id);
        if (existingPlayer == null)
            return null;

        existingPlayer.Name = player.Name;
        existingPlayer.Position = player.Position;
        existingPlayer.ImageUrl = player.ImageUrl;
        existingPlayer.Description = player.Description;
        existingPlayer.Price = player.Price;
        existingPlayer.IsAvailable = player.IsAvailable;

        await _context.SaveChangesAsync();
        return existingPlayer;
    }

    public async Task<bool> DeletePlayer(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player == null)
            return false;

        _context.Players.Remove(player);
        await _context.SaveChangesAsync();
        return true;
    }
} 