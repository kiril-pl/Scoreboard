namespace SportRadar.FootballCup.Scoreboard;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Models;

public class Scoreboard : IScoreboard
{
    private readonly IScoreboardValidator _scoreboardValidator;
    private readonly List<Match> _matches = [];

    public Scoreboard(IScoreboardValidator scoreboardValidator)
    {
        _scoreboardValidator = scoreboardValidator;
    }

    public Result StartMatch(string homeTeam, string awayTeam)
    {
        var validationResult = _scoreboardValidator.ValidateStartMatch(this, homeTeam, awayTeam);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        _matches.Add(new(homeTeam, awayTeam));
        return Result.Ok();
    }

    public Result UpdateMatchScore(TeamScore homeTeam, TeamScore awayTeam)
    {
        var validationResult = _scoreboardValidator.ValidateUpdateMatchScore(this, homeTeam, awayTeam);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var findResult = ScoreboardUtils.FindMatchByTeamNames(_matches, homeTeam.Name, awayTeam.Name);
        findResult.Value.HomeTeam = homeTeam;
        findResult.Value.AwayTeam = awayTeam;

        return Result.Ok();
    }

    public Result FinishMatch(string homeTeam, string awayTeam)
    {
        var validationResult = _scoreboardValidator.ValidateFinishMatch(this, homeTeam, awayTeam);
        if (validationResult.IsFailed)
        {
            return validationResult;
        }

        var matchResult = ScoreboardUtils.FindMatchByTeamNames(_matches, homeTeam, awayTeam);
        _matches.Remove(matchResult.Value);

        return Result.Ok();
    }

    public List<Match> GetMatchesSummary() =>
        _matches
            .OrderByDescending(m => m.HomeTeam.Score + m.AwayTeam.Score)
            .ThenByDescending(m => m.StartTime)
            .ToList();
}
