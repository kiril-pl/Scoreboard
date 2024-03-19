namespace SportRadar.FootballCup.Tests;

using Microsoft.Extensions.DependencyInjection;
using SportRadar.FootballCup.Scoreboard;
using SportRadar.FootballCup.Scoreboard.Contracts;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddScoped<IScoreboard, FootballCup.Scoreboard.Scoreboard>();
        services.AddScoped<IScoreboardValidator, ScoreboardValidator>();
    }
}
