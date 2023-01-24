using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

[Collection("GameTests")]
public class GameTests : BaseTest
{
  public GameTests(TestWebApplicationFactory<Program> factory) : base(factory) { }

  [Fact]
  public async Task CreateNewGame()
  {
    const string VALID_TEAM_ID = "valid id";
    var client = await Given(context =>
    {
      context.Teams.Add(new Team
      {
        Id = VALID_TEAM_ID,
        Name = "Team title"
      });
    });

    // When a new game is created
    var response = await client.PostAsJsonAsync("/game", new NewGame
    {
      TeamId = VALID_TEAM_ID
    });

    Then(async context =>
    {
      // Then the game is added to the database.
      var createdGame = context!.Games.First();
      Assert.Equal(createdGame.TeamId, VALID_TEAM_ID);
      Assert.Equal(createdGame.State, "INPROGRESS");
      Assert.NotNull(createdGame.Word);
      Assert.Null(createdGame.Guess1);
      Assert.Null(createdGame.Guess2);
      Assert.Null(createdGame.Guess3);
      Assert.Null(createdGame.Guess4);
      Assert.Null(createdGame.Guess5);
      Assert.Null(createdGame.Guess6);

      // Then the details of the game are returned
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      var detail = await response.Content.ReadFromJsonAsync<NewGameResponse>();
      Assert.NotNull(detail);
      Assert.Equal(createdGame.Id, detail.GameId);
    });

  }

  [Fact]
  public async Task TheTeamMustBeRegisteredBeforeAGameCanBeCreated()
  {
    var client = await Given();

    // When an attempt is made to create a game for a made up team
    const string INVALID_TEAM_ID = "made up";
    var response = await client.PostAsJsonAsync("/game", new NewGame
    {
      TeamId = INVALID_TEAM_ID
    });

    Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
  }
}