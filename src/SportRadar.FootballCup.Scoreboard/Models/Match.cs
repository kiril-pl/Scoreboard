namespace SportRadar.FootballCup.Scoreboard.Models;

public class Match(string homeTeam, string awayTeam)
{
    public TeamScore HomeTeam { get; set; } = new(homeTeam, 0);

    public TeamScore AwayTeam { get; set; } = new(awayTeam, 0);

    public DateTime StartTime { get; } = DateTime.UtcNow;
}
