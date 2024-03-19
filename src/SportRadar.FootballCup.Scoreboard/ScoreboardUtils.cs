namespace SportRadar.FootballCup.Scoreboard;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Errors;
using SportRadar.FootballCup.Scoreboard.Models;

public static class ScoreboardUtils
{
    public static Result<Match> FindMatchByTeamNames(
        IReadOnlyList<Match> matches, string homeTeam, string awayTeam)
    {
        var match = matches.FirstOrDefault(
            m => m.HomeTeam.Name == homeTeam && m.AwayTeam.Name == awayTeam);

        return match == null
            ? Result.Fail(new MatchDoesNotExistError(homeTeam, awayTeam))
            : Result.Ok(match);
    }
}
