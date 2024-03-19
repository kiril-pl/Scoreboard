namespace SportRadar.FootballCup.Tests.Scoreboard;

using FluentAssertions;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Errors;

public class UpdateMatchScoreTests
{
    private readonly IScoreboard _scoreboard;

    public UpdateMatchScoreTests(IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
    }

    [Theory]
    [InlineData("TeamA", 2, "TeamB", 1)]
    public void UpdateScore_WithExistingMatch_ShouldUpdateItsScore(
        string homeTeam, int homeScore, string awayTeam, int awayScore)
    {
        _scoreboard.StartMatch(homeTeam, awayTeam);

        _scoreboard.UpdateMatchScore(new(homeTeam, homeScore), new(awayTeam, awayScore));

        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Name.Should().Be(homeTeam);
                first.HomeTeam.Score.Should().Be(homeScore);
                first.AwayTeam.Name.Should().Be(awayTeam);
                first.AwayTeam.Score.Should().Be(awayScore);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }

    [Theory]
    [InlineData("TeamA", 3, "TeamB", 5)]
    public void UpdateScore_WithNonExistingMatch_MatchDoesNotExistErrorReturned(
        string homeTeam, int homeScore, string awayTeam, int awayScore)
    {
        (string currentHomeTeam, string currentAwayTeam) = ("Team1", "Team2");
        _scoreboard.StartMatch(currentHomeTeam, currentAwayTeam);

        var result = _scoreboard.UpdateMatchScore(new(homeTeam, homeScore), new(awayTeam, awayScore));

        result.Errors.OfType<MatchDoesNotExistError>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Score.Should().Be(0);
                first.HomeTeam.Name.Should().Be(currentHomeTeam);
                first.AwayTeam.Score.Should().Be(0);
                first.AwayTeam.Name.Should().Be(currentAwayTeam);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }

    [Theory]
    [InlineData("TeamA", -3, "TeamB", 2)]
    [InlineData("TeamA", 3, "TeamB", -2)]
    [InlineData("TeamA", -3, "TeamB", -2)]
    public void UpdateScore_WithNegativeValues_NewTeamScoreIsLowerErrorReturned(
        string homeTeam, int homeScore, string awayTeam, int awayScore)
    {
        _scoreboard.StartMatch(homeTeam, awayTeam);

        var result = _scoreboard.UpdateMatchScore(new(homeTeam, homeScore), new(awayTeam, awayScore));

        result.Errors.OfType<NewTeamScoreIsLowerThanBefore>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Score.Should().Be(0);
                first.HomeTeam.Name.Should().Be(homeTeam);
                first.AwayTeam.Score.Should().Be(0);
                first.AwayTeam.Name.Should().Be(awayTeam);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }

    [Theory]
    [InlineData("TeamA", +1, "TeamB", -1)]
    [InlineData("TeamA", -1, "TeamB", +1)]
    [InlineData("TeamA", -1, "TeamB", -1)]
    public void UpdateScore_WithLowerValues_NewTeamScoreIsLowerErrorReturned(
        string homeTeam, int homeScoreDiff, string awayTeam, int awayScoreDiff)
    {
        (int startHomeScore, int startAwayScore) = (3, 2);
        (int newHomeScore, int newAwayScore) = (startHomeScore + homeScoreDiff, startAwayScore + awayScoreDiff);
        _scoreboard.StartMatch(homeTeam, awayTeam);

        _scoreboard.UpdateMatchScore(new(homeTeam, startHomeScore), new(awayTeam, startAwayScore));
        var result = _scoreboard.UpdateMatchScore(new(homeTeam, newHomeScore), new(awayTeam, newAwayScore));

        result.Errors.OfType<NewTeamScoreIsLowerThanBefore>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Score.Should().Be(startHomeScore);
                first.HomeTeam.Name.Should().Be(homeTeam);
                first.AwayTeam.Score.Should().Be(startAwayScore);
                first.AwayTeam.Name.Should().Be(awayTeam);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }
}

