namespace FootballClub_Backend.Models.Responses;

public class MatchDto
{
    public string Id { get; set; } = string.Empty;
    public string HomeTeam { get; set; } = string.Empty;
    public string AwayTeam { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Venue { get; set; } = string.Empty;
    public string Competition { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public MatchScoreDto? Score { get; set; }
    public int TicketsAvailable { get; set; }

    public MatchDto() { }

    public MatchDto(Entities.Match match)
    {
        Id = match.Id;
        HomeTeam = match.HomeTeam;
        AwayTeam = match.AwayTeam;
        Date = match.Date;
        Venue = match.Venue;
        Competition = match.Competition;
        Status = match.Status;
        TicketsAvailable = match.TicketsAvailable;
        
        if (match.Score != null)
        {
            Score = new MatchScoreDto
            {
                HomeScore = match.Score.HomeScore,
                AwayScore = match.Score.AwayScore
            };
        }
    }
}

public class MatchScoreDto
{
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
} 