namespace SportRadar.FootballCup.Scoreboard.Models;

public class TeamScore(string name, int score)
{
    public string Name { get; } = name;

    public int Score { get; set; } = score;
}
