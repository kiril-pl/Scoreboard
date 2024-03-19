namespace SportRadar.FootballCup.Scoreboard;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Models;

public class ScoreboardValidator : IScoreboardValidator
{
    public Result ValidateStartMatch(IScoreboard scoreboard, string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }

    public Result ValidateUpdateMatchScore(IScoreboard scoreboard, TeamScore homeTeam, TeamScore awayTeam)
    {
        throw new NotImplementedException();
    }

    public Result ValidateFinishMatch(IScoreboard scoreboard, string homeTeam, string awayTeam)
    {
        throw new NotImplementedException();
    }
}
