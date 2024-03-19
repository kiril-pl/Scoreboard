namespace SportRadar.FootballCup.Tests.Scoreboard;

using FluentAssertions;
using SportRadar.FootballCup.Scoreboard.Contracts;
using SportRadar.FootballCup.Scoreboard.Models;

public class GetMatchesSummaryTests
{
    private readonly IScoreboard _scoreboard;

    public GetMatchesSummaryTests(IScoreboard scoreboard)
    {
        _scoreboard = scoreboard;
    }

    [Fact]
    public void GetSummary_WithSampleMatches_ShouldReturnMatchesOrderedByTotalScoreAndStartTime()
    {
        PlaySampleMatches();

        var matches = _scoreboard.GetMatchesSummary();
        AssertSampleScoresAreCorrect(matches);
        AssertSampleSameScoreGroupsAreOrderedCorrectly(matches);
    }

    [Fact]
    public void GetSummary_WithNoOngoingMatches_ShouldReturnEmptyList()
    {
        _scoreboard.StartMatch("TeamA", "TeamB");
        _scoreboard.FinishMatch("TeamA", "TeamB");

        _scoreboard.GetMatchesSummary().Should().BeEmpty();
    }

    [Fact]
    public void GetSummary_WithMultipleMatchesAndSameTotalScore_ShouldReturnMatchesOrderedByStartTime()
    {
        _scoreboard.StartMatch("TeamA", "TeamB");
        _scoreboard.StartMatch("TeamC", "TeamD");
        _scoreboard.StartMatch("TeamE", "TeamF");
        _scoreboard.UpdateMatchScore(new("TeamA", 2), new("TeamB", 1));
        _scoreboard.UpdateMatchScore(new("TeamC", 1), new("TeamD", 2));
        _scoreboard.UpdateMatchScore(new("TeamE", 0), new("TeamF", 0));

        var matches = _scoreboard.GetMatchesSummary();

        AssertAreEqual(matches[0], new("TeamC", 1), new("TeamD", 2));
        AssertAreEqual(matches[1], new("TeamA", 2), new("TeamB", 1));
        AssertAreEqual(matches[2], new("TeamE", 0), new("TeamF", 0));
        AssertSampleSameScoreGroupsAreOrderedCorrectly(matches);
    }

    [Fact]
    public void GetSummary_WithMultipleMatchesAndDifferentTotalScore_ShouldReturnMatchesOrderedByTotalScore()
    {
        _scoreboard.StartMatch("TeamA", "TeamB");
        _scoreboard.StartMatch("TeamC", "TeamD");
        _scoreboard.StartMatch("TeamE", "TeamF");
        _scoreboard.UpdateMatchScore(new("TeamC", 1), new("TeamD", 3));
        _scoreboard.UpdateMatchScore(new("TeamA", 5), new("TeamB", 2));
        _scoreboard.UpdateMatchScore(new("TeamE", 2), new("TeamF", 1));

        var matches = _scoreboard.GetMatchesSummary();

        AssertAreEqual(matches[0], new("TeamA", 5), new("TeamB", 2));
        AssertAreEqual(matches[1], new("TeamC", 1), new("TeamD", 3));
        AssertAreEqual(matches[2], new("TeamE", 2), new("TeamF", 1));
        AssertSampleSameScoreGroupsAreOrderedCorrectly(matches);
    }

    private void AssertAreEqual(Match match, TeamScore home, TeamScore away)
    {
        match.Should().Match<Match>(m =>
            m.HomeTeam.Name == home.Name && m.HomeTeam.Score == home.Score &&
            m.AwayTeam.Name == away.Name && m.AwayTeam.Score == away.Score);
    }

    private void AssertSampleSameScoreGroupsAreOrderedCorrectly(List<Match> matches)
    {
        matches.GroupBy(m => m.StartTime)
            .ToList()
            .ForEach(g =>
            {
                var orderedMatches = g.OrderBy(m => m.StartTime).ToList();
                orderedMatches.Should().Equal(g);
            });
    }

    private void AssertSampleScoresAreCorrect(List<Match> matches)
    {
        matches.Should().HaveCount(5);
        AssertAreEqual(matches[0], new("Uruguay", 6), new("Italy", 6));
        AssertAreEqual(matches[1], new("Spain", 10), new("Brazil", 2));
        AssertAreEqual(matches[2], new("Mexico", 0), new("Canada", 5));
        AssertAreEqual(matches[3], new("Argentina", 3), new("Australia", 1));
        AssertAreEqual(matches[4], new("Germany", 2), new("France", 2));
    }

    private void PlaySampleMatches()
    {
        _scoreboard.StartMatch("Mexico", "Canada");
        _scoreboard.StartMatch("Spain", "Brazil");
        _scoreboard.UpdateMatchScore(new("Mexico", 0), new("Canada", 5));
        _scoreboard.StartMatch("Germany", "France");
        _scoreboard.UpdateMatchScore(new("Spain", 9), new("Brazil", 2));
        _scoreboard.UpdateMatchScore(new("Spain", 10), new("Brazil", 2));
        _scoreboard.UpdateMatchScore(new("Germany", 2), new("France", 2));
        _scoreboard.StartMatch("Uruguay", "Italy");
        _scoreboard.StartMatch("Argentina", "Australia");
        _scoreboard.UpdateMatchScore(new("Uruguay", 5), new("Italy", 4));
        _scoreboard.UpdateMatchScore(new("Argentina", 2), new("Australia", 1));
        _scoreboard.UpdateMatchScore(new("Argentina", 3), new("Australia", 1));
        _scoreboard.UpdateMatchScore(new("Uruguay", 6), new("Italy", 6));
    }
}
