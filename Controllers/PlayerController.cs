using Microsoft.AspNetCore.Mvc;
using FootballClub_Backend.Services;
using FootballClub_Backend.Models;
using Microsoft.AspNetCore.Authorization;

namespace FootballClub_Backend.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PlayerController : ControllerBase
{
    private readonly IPlayerService _playerService;

    public PlayerController(IPlayerService playerService)
    {
        _playerService = playerService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Player>>> GetAllPlayers()
    {
        var players = await _playerService.GetAllPlayers();
        return Ok(players);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Player>> GetPlayer(int id)
    {
        var player = await _playerService.GetPlayerById(id);
        if (player == null)
            return NotFound();

        return Ok(player);
    }

    [Authorize(Roles = "admin")]
    [HttpPost]
    public async Task<ActionResult<Player>> CreatePlayer(Player player)
    {
        var newPlayer = await _playerService.CreatePlayer(player);
        return CreatedAtAction(nameof(GetPlayer), new { id = newPlayer.Id }, newPlayer);
    }

    [Authorize(Roles = "admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<Player>> UpdatePlayer(int id, Player player)
    {
        var updatedPlayer = await _playerService.UpdatePlayer(id, player);
        if (updatedPlayer == null)
            return NotFound();

        return Ok(updatedPlayer);
    }

    [Authorize(Roles = "admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeletePlayer(int id)
    {
        var result = await _playerService.DeletePlayer(id);
        if (!result)
            return NotFound();

        return NoContent();
    }
} 