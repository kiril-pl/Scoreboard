namespace SportRadar.FootballCup.Tests.Scoreboard;

using FluentAssertions;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Errors;

public class StartMatchTests
{
    private readonly IScoreboard _scoreboard;

    public StartMatchTests(IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
    }

    [Theory]
    [InlineData("TeamA", "TeamB")]
    public void StartMatch_WithValidTeams_ShouldAddMatchToScoreboard(string homeTeam, string awayTeam)
    {
        _scoreboard.StartMatch(homeTeam, awayTeam);

        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Name.Should().Be(homeTeam);
                first.HomeTeam.Score.Should().Be(0);
                first.AwayTeam.Name.Should().Be(awayTeam);
                first.AwayTeam.Score.Should().Be(0);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }

    [Theory]
    [InlineData("", "TeamB")]
    [InlineData("TeamA", "")]
    public void StartMatch_WithEitherEmptyTeam_EmptyErrorTeamReturned(string homeTeam, string awayTeam)
    {
        var result = _scoreboard.StartMatch(homeTeam, awayTeam);

        result.Errors.OfType<MatchTeamsCannotBeEmpty>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().BeEmpty();
    }

    [Theory]
    [InlineData("TeamA", "TeamA")]
    public void StartMatch_WithDuplicateTeams_DuplicateTeamErrorReturned(string homeTeam, string awayTeam)
    {
        var result = _scoreboard.StartMatch(homeTeam, awayTeam);

        result.IsFailed.Should().BeTrue();
        result.Errors.OfType<MatchHasDuplicateTeamNames>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().BeEmpty();
    }

    [Theory]
    [InlineData("TeamA", "TeamB", "TeamC", "TeamA")]
    [InlineData("TeamA", "TeamB", "TeamA", "TeamC")]
    [InlineData("TeamA", "TeamB", "TeamB", "TeamA")]
    public void StartMatch_WithAlreadyPlayingTeam_TeamIsAlreadyPlayingErrorReturned(
        string existingHomeTeam, string existingAwayTeam, string newHomeTeam, string newAwayTeam)
    {
        _scoreboard.StartMatch(existingHomeTeam, existingAwayTeam);

        var result = _scoreboard.StartMatch(newHomeTeam, newAwayTeam);

        result.Errors.OfType<TeamIsAlreadyPlaying>().Should().NotBeEmpty();
        _scoreboard.GetMatchesSummary().Should().HaveCount(1)
            .And.SatisfyRespectively(first =>
            {
                first.HomeTeam.Name.Should().Be(existingHomeTeam);
                first.HomeTeam.Score.Should().Be(0);
                first.AwayTeam.Name.Should().Be(existingAwayTeam);
                first.AwayTeam.Score.Should().Be(0);
                first.StartTime.Date.Should().Be(DateTime.UtcNow.Date);
            });
    }
}
