namespace SportRadar.FootballCup.Scoreboard.Contracts;

using FluentResults;
using SportRadar.FootballCup.Scoreboard.Models;

public interface IScoreboard
{
    Result StartMatch(string homeTeam, string awayTeam);

    Result UpdateMatchScore(TeamScore homeTeam, TeamScore awayTeam);

    Result FinishMatch(string homeTeam, string awayTeam);

    List<Match> GetMatchesSummary();
}
