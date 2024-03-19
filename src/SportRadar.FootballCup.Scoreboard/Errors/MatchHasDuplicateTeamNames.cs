namespace SportRadar.FootballCup.Scoreboard.Errors;

using FluentResults;

public class MatchHasDuplicateTeamNames(string homeTeam, string awayTeam)
    : Error($"Match teams names cannot be the same (e.g. {homeTeam} != {awayTeam}");

