namespace SportRadar.FootballCup.Scoreboard.Contracts;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Models;

public interface IScoreboardValidator
{
    Result ValidateStartMatch(IScoreboard scoreboard, string homeTeam, string awayTeam);

    Result ValidateUpdateMatchScore(IScoreboard scoreboard, TeamScore homeTeam, TeamScore awayTeam);

    Result ValidateFinishMatch(IScoreboard scoreboard, string homeTeam, string awayTeam);
}

