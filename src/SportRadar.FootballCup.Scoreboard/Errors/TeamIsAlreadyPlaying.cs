namespace SportRadar.FootballCup.Scoreboard.Errors;

using FluentResults;

public class TeamIsAlreadyPlaying(string teamName)
    : Error($"Team of {teamName} is already playing");
