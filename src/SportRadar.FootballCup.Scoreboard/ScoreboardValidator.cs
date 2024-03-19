namespace SportRadar.FootballCup.Scoreboard;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Errors;
using SportRadar.FootballCup.Scoreboard.Models;

public class ScoreboardValidator : IScoreboardValidator
{
    public Result ValidateStartMatch(IScoreboard scoreboard, string homeTeam, string awayTeam)
    {
        var namesValidation = ValidateTeamNamesAreNotEmpty(homeTeam, awayTeam);
        if (namesValidation.IsFailed)
        {
            return namesValidation;
        }

        if (homeTeam == awayTeam)
        {
            return Result.Fail(new MatchHasDuplicateTeamNames(homeTeam, awayTeam));
        }

        List<Result> alreadyPlayingValidations =
        [
            ValidateTeamIsNotAlreadyPlaying(scoreboard.GetMatchesSummary(), homeTeam),
            ValidateTeamIsNotAlreadyPlaying(scoreboard.GetMatchesSummary(), awayTeam),
        ];
        if (alreadyPlayingValidations.Any(v => v.IsFailed))
        {
            return alreadyPlayingValidations.Merge();
        }

        return Result.Ok();
    }

    public Result ValidateUpdateMatchScore(IScoreboard scoreboard, TeamScore homeTeam, TeamScore awayTeam)
    {
        var namesValidation = ValidateTeamNamesAreNotEmpty(homeTeam.Name, awayTeam.Name);
        if (namesValidation.IsFailed)
        {
            return namesValidation;
        }

        var findResult = ScoreboardUtils.FindMatchByTeamNames(
            scoreboard.GetMatchesSummary(), homeTeam.Name, awayTeam.Name);

        if (findResult.IsFailed)
        {
            return Result.Fail(findResult.Errors);
        }

        var scoreValidation = ValidateNewScoreIsNotLowerThanBefore(scoreboard.GetMatchesSummary(), homeTeam, awayTeam);
        if (scoreValidation.IsFailed)
        {
            return scoreValidation;
        }

        return Result.Ok();
    }

    public Result ValidateFinishMatch(IScoreboard scoreboard, string homeTeam, string awayTeam)
    {
        var matchResult = ScoreboardUtils.FindMatchByTeamNames(scoreboard.GetMatchesSummary(), homeTeam, awayTeam);
        return matchResult.ValueOrDefault is null
            ? Result.Fail(new MatchDoesNotExistError(homeTeam, awayTeam))
            : Result.Ok();
    }

    private Result ValidateTeamNamesAreNotEmpty(string homeTeam, string awayTeam)
    {
        if (string.IsNullOrEmpty(homeTeam) || string.IsNullOrEmpty(awayTeam))
        {
            return Result.Fail(new MatchTeamsCannotBeEmpty(homeTeam, awayTeam));
        }

        return Result.Ok();
    }

    private Result ValidateTeamIsNotAlreadyPlaying(IReadOnlyList<Match> matches, string teamName)
    {
        if (matches.Any(m => m.HomeTeam.Name == teamName || m.AwayTeam.Name == teamName))
        {
            return Result.Fail(new TeamIsAlreadyPlaying(teamName));
        }

        return Result.Ok();
    }

    private Result ValidateNewScoreIsNotLowerThanBefore(
        IReadOnlyList<Match> matches, TeamScore homeTeam, TeamScore awayTeam)
    {
        var findResult = ScoreboardUtils.FindMatchByTeamNames(matches, homeTeam.Name, awayTeam.Name);
        if (findResult.Value.HomeTeam.Score > homeTeam.Score)
        {
            return Result.Fail(new NewTeamScoreIsLowerThanBefore(
                homeTeam.Name, findResult.Value.HomeTeam.Score, homeTeam.Score));
        }

        if (findResult.Value.AwayTeam.Score > awayTeam.Score)
        {
            return Result.Fail(new NewTeamScoreIsLowerThanBefore(
                awayTeam.Name, findResult.Value.AwayTeam.Score, awayTeam.Score));
        }

        return Result.Ok();
    }
}

