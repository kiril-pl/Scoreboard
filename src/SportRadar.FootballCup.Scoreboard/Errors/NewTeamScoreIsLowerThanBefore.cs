namespace SportRadar.FootballCup.Scoreboard.Errors;

using FluentResults;

public class NewTeamScoreIsLowerThanBefore(string teamName, int currentScore, int newScore)
    : Error($"New team score ({newScore}) is lower than current score ({currentScore} for team {teamName}");
