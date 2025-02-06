namespace FootballClub_Backend.Models.Responses;

public class PlayerDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Nationality { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public string? ImageUrl { get; set; }
    public PlayerStatsDto Stats { get; set; } = new();

    public PlayerDto() { }

    public PlayerDto(Entities.Player player)
    {
        Id = player.Id;
        Name = player.Name;
        Position = player.Position;
        Number = player.Number;
        Nationality = player.Nationality;
        DateOfBirth = player.DateOfBirth;
        ImageUrl = player.ImageUrl;
        Stats = new PlayerStatsDto
        {
            Appearances = player.Stats.Appearances,
            Goals = player.Stats.Goals,
            Assists = player.Stats.Assists,
            CleanSheets = player.Stats.CleanSheets
        };
    }
}

public class PlayerStatsDto
{
    public int Appearances { get; set; }
    public int Goals { get; set; }
    public int Assists { get; set; }
    public int CleanSheets { get; set; }
} 