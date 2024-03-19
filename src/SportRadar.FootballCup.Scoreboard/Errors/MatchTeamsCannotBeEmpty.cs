namespace SportRadar.FootballCup.Scoreboard.Errors;

using FluentResults;

public class MatchTeamsCannotBeEmpty(string homeTeam, string awayTeam)
    : Error($"Match teams cannot be empty: home team = {homeTeam}, away team = {awayTeam}");

