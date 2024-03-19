namespace SportRadar.FootballCup.Scoreboard;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Models;

public class Scoreboard : IScoreboard
{
    public Result StartMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public Result UpdateMatchScore(TeamScore homeTeam, TeamScore awayTeam)
    {
        throw new NotImplementedException();
    }

    public Result FinishMatch(string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public List<Match> GetMatchesSummary()
    {
        throw new NotImplementedException();
    }
}
