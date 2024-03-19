namespace SportRadar.FootballCup.Scoreboard.Errors;

using FluentResults;

public class MatchDoesNotExistError(string homeTeam, string awayTeam)
    : Error($"Cannot find the match of home/away teams {homeTeam}/{awayTeam}");

