using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

public class GameTests : BaseTest
{
  public GameTests(TestWebApplicationFactory<Program> factory) : base(factory) { }

  [Fact]
  public async Task CreateNewGame()
  {
    var team = await GivenATeam();

    var client = Client();
    var response = await client.PostAsJsonAsync("/game", new NewGame
    {
      TeamId = team.Id
    });

    // Then the details of the game are returned
    await ThenOKIsReturned(response);
    var detail = await response.Content.ReadFromJsonAsync<NewGameResponse>();
    Assert.NotNull(detail);
    // Assert.Equal(createdGame.Id, detail.GameId);

    // When a new game is created
    Then(context =>
    {
      // Then the game is added to the database.
      var createdGame = context!.Games.First();
      // Assert.Equal(createdGame.Team.Id, VALID_TEAM_ID);
      Assert.Equal(createdGame.State, GameState.InProgress);
      Assert.NotNull(createdGame.Word);
      Assert.Null(createdGame.Guess1);
      Assert.Null(createdGame.Guess2);
      Assert.Null(createdGame.Guess3);
      Assert.Null(createdGame.Guess4);
      Assert.Null(createdGame.Guess5);
      Assert.Null(createdGame.Guess6);
    });
  }

  [Fact]
  public async Task TheTeamMustBeRegisteredBeforeAGameCanBeCreated()
  {
    var client = Client();

    // When an attempt is made to create a game for a made up team
    var INVALID_TEAM_ID = Guid.NewGuid();
    var response = await client.PostAsJsonAsync("/game", new NewGame
    {
      TeamId = INVALID_TEAM_ID
    });

    await ThenNotFoundIsReturned(response, "Team does not exist. Please call Team first.");

  }
}