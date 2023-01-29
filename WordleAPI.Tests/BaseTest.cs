using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
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
      var result = builder(db!);
      await db!.SaveChangesAsync();
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
                                        Action<Game>? builder = null,
                                        Team? team = null)
  {
    return await Given(context =>
    {
      if (team is null)
      {
        team = new Team
        {
          Id = Guid.NewGuid(),
          Name = "Team title"
        };
        context.Teams.Add(team);
      }

      var game = new Game
      {
        Id = Guid.NewGuid(),
        TeamId = team.Id,
        Word = word
      };

      if (builder is not null)
      {
        builder(game);
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

  protected static async Task ThenOKIsReturned(HttpResponseMessage response)
  {
    if (response.StatusCode != HttpStatusCode.OK)
    {
      var detail = await response.Content.ReadAsStringAsync();
      Assert.Fail(detail);
    }
  }

  protected static async Task ThenAValidationProblemIsReturned(HttpResponseMessage response,
                                                               string expectedDetail)
  {
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
    Assert.Equal(expectedDetail, detail!.Detail);
  }

  protected static async Task ThenAFieldValidationProblemIsReturned(HttpResponseMessage response,
                                                                    string fieldName,
                                                                    string expectedMessage)
  {
    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<HttpValidationProblemDetails>();
    Assert.Equal(expectedMessage, detail!.Errors[fieldName][0]);
  }

  protected static async Task ThenNotFoundIsReturned(HttpResponseMessage response, string expectedMessage)
  {
    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<string>();
    Assert.Equal(expectedMessage, detail);
  }

}
