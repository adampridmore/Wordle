using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

public class GuessTests : BaseTest
{
  public GuessTests(TestWebApplicationFactory<Program> factory) : base(factory) { }

  [Fact]
  public async Task GuessesMustBeValidWords()
  {
    var TEAM_ID = Guid.NewGuid();
    var GAME_ID = Guid.NewGuid();
    await Given(context =>
    {
      var team = new Team
      {
        Id = TEAM_ID,
        Name = "Team title"
      };
      var game = new Game
      {
        Id = GAME_ID,
        Team = team,
        Word = "APPLE"
      };
      context.Games.Add(game);
    });
    var client = Client();
    var response = await client.PostAsJsonAsync("/guess", new NewGuess
    {
      GameId = GAME_ID,
      Guess = "ABCDE"
    });

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<string>();
    Assert.Equal("Your guess is not a valid word", detail);
  }
}
