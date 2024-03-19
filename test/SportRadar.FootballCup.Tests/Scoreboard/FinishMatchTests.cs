namespace SportRadar.FootballCup.Tests.Scoreboard;

using FluentAssertions;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Errors;

public class FinishMatchTests
{
    private readonly IScoreboard _scoreboard;

    public FinishMatchTests(IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
    }

    [Theory]
    [InlineData("TeamA", "TeamB")]
    public void FinishMatch_WithExistingMatch_ShouldRemoveMatchFromScoreboard(
        string homeTeam, string awayTeam)
    {
        _scoreboard.StartMatch(homeTeam, awayTeam);

        _scoreboard.FinishMatch(homeTeam, awayTeam);

        _scoreboard.GetMatchesSummary()
            .Where(s => s.HomeTeam.Name == homeTeam && s.AwayTeam.Name == awayTeam)
            .Should()
            .BeEmpty();
        _scoreboard.GetMatchesSummary().Should().BeEmpty();
    }

    [Theory]
    [InlineData("TeamA", "TeamB")]
    public void FinishMatch_WithNonExistingMatch_MatchDoesNotExistErrorReturned(
        string homeTeam, string awayTeam)
    {
        _scoreboard.StartMatch(homeTeam, awayTeam);

        var result = _scoreboard.FinishMatch("one", "two");

        result.Errors.OfType<MatchDoesNotExistError>().Should().NotBeEmpty();
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
}
