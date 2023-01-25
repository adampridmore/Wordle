using Microsoft.Extensions.DependencyInjection;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

[Collection("all")]
public abstract class BaseTest : IClassFixture<TestWebApplicationFactory<Program>>
{
  private readonly TestWebApplicationFactory<Program> _factory;
  private readonly HttpClient _httpClient;

  public BaseTest(TestWebApplicationFactory<Program> factory)
  {
    _factory = factory;
    _httpClient = factory.CreateClient();

    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      db!.Database.EnsureDeleted();
      db.Database.EnsureCreated();
    }

  }

  protected void Then(Action<WordleDb> check)
  {
    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      check(db!);
    }
  }

  protected async Task<T> Given<T>(Func<WordleDb, T> builder) where T : notnull
  {
    using (var scope = _factory.Services.CreateScope())
    {
      var db = scope.ServiceProvider.GetService<WordleDb>();
      var result = builder(db);
      await db.SaveChangesAsync();
      return result;
    }
  }

  protected HttpClient Client()
  {
    var client = _factory.WithWebHostBuilder(builder =>
  {
  }).CreateClient();
    return client;
  }

  protected async Task<Game> GivenAGame(string word, 
                                        string[]? wrongGuesses = null)
  {
    return await Given(context =>
    {
      var team = new Team
      {
        Id = Guid.NewGuid(),
        Name = "Team title"
      };
      context.Teams.Add(team);

      var game = new Game
      {
        Id = Guid.NewGuid(),
        Team = team,
        Word = word
      };

      if (wrongGuesses is not null)
      {
        if (wrongGuesses.Length > 0)
          game.Guess1 = wrongGuesses[0];
        if (wrongGuesses.Length > 1)
          game.Guess2 = wrongGuesses[1];
        if (wrongGuesses.Length > 2)
          game.Guess3 = wrongGuesses[2];
        if (wrongGuesses.Length > 3)
          game.Guess4 = wrongGuesses[3];
        if (wrongGuesses.Length > 4)
          game.Guess5 = wrongGuesses[4];
        if (wrongGuesses.Length > 5)
          game.Guess6 = wrongGuesses[5];
      }

      context.Games.Add(game);
      return game;
    });
  }

  protected async Task<Team> GivenATeam()
  {
    return await Given(context =>
    {
      var team = new Team
      {
        Id = Guid.NewGuid(),
        Name = "Team title"
      };
      context.Teams.Add(team);
      return team;
    });
  }

}
