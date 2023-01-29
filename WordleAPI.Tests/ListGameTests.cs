using System.Net.Http.Json;
using WordleAPI.Tests.Helpers;
using Xunit;

namespace WordleAPI.Tests;

public class ListGameTests : BaseTest
{
  public ListGameTests(TestWebApplicationFactory<Program> factory) : base(factory) { }

  [Fact]
  public async Task TheTeamMustBeRegisteredBeforeAGameCanBeCreated()
  {
    var INVALID_TEAM_ID = Guid.NewGuid();
    var client = Client();
    var response = await client.GetAsync($"/team/{INVALID_TEAM_ID}/games");
    await ThenNotFoundIsReturned(response, "The team does not exist.");
  }

  [Fact]
  public async Task TheGamesForATeamCanBeRetrieved()
  {
    var yesterday = DateTime.Now.AddDays(-1);
    var today = DateTime.Now;
    
    // Given a team with 2 games
    var team  = await GivenATeam();
    var expectedGame1 = await GivenAGame(word: "ABCDE",
                                with =>
                                {
                                  with.State = GameState.Won;
                                  with.DateStarted = yesterday;
                                }, team);
    var expectedGame2 = await GivenAGame(word: "ABCDE",
                                with =>
                                {
                                  with.State = GameState.Lost;
                                  with.DateStarted = today;
                                }, team);

    // Given another team with a games
    var otherGame = await GivenAGame(word: "ABCDE");

    var client = Client();

    var actualGames = await client.GetFromJsonAsync<GetGamesResponse[]>($"/team/{team.Id}/games");
    Assert.Equal(2, actualGames!.Count());

    var actualGame1 = actualGames![0];
    Assert.Equal(expectedGame1.DateStarted, actualGame1.DateStarted);
    Assert.Equal(expectedGame1.Id, actualGame1.GameId);
    Assert.Equal(expectedGame1.State, actualGame1.State);

    var actualGame2 = actualGames![1];
    Assert.Equal(expectedGame2.DateStarted, actualGame2.DateStarted);
    Assert.Equal(expectedGame2.Id, actualGame2.GameId);
    Assert.Equal(expectedGame2.State, actualGame2.State);

  }
}