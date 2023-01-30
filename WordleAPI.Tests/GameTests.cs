using System.Net.Http.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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

    var response = await WhenANewGameIsCreated(team.Id);

    await ThenOKIsReturned(response);

    // Then the details of the game are returned
    var detail = await response.Content.ReadFromJsonAsync<NewGameResponse>();
    Assert.NotNull(detail);

    // When a new game is created
    Then(context =>
    {
      // Then the game is added to the database.
      var createdGame = context.Games.Include(game => game.Team).First();
      Assert.Equal(createdGame.Team!.Id, team.Id);
      Assert.Equal(createdGame.Id, detail.GameId);
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
    // When an attempt is made to create a game for a made up team
    var response = await WhenANewGameIsCreated(teamId: Guid.NewGuid());

    await ThenNotFoundIsReturned(response, "Team does not exist. Please call Team first.");
  }

  [Fact]
  public async Task AGameCanBeRetrieved()
  {
    var currentDate = DateTime.Now;
    var game = await GivenAGame(word: "ABCDE",
                                with =>
                                {
                                  with.Guess1 = "AAAAA";
                                  with.Guess2 = "BBBBB";
                                  with.Guess3 = "CCCCC";
                                  with.Guess4 = "DDDDD";
                                  with.Guess5 = "EEEEE";
                                  with.Guess6 = "FFFFF";
                                  with.State = GameState.Lost;
                                  with.DateStarted = currentDate;
                                });

    var actualGame = await WhenTheGameIsRetrieved(game);

    Assert.NotNull(actualGame);
    Assert.Equal(game.Id, actualGame.GameId);
    Assert.Equal("ABCDE", actualGame.Word);
    Assert.Equal(GameState.Lost, actualGame.State);
    Assert.Equal(currentDate, actualGame.DateStarted);

    Assert.Equal("AAAAA", actualGame.Guesses[0].Guess);
    Assert.Equal("GYYYY", actualGame.Guesses[0].Score);

    Assert.Equal("BBBBB", actualGame.Guesses[1].Guess);
    Assert.Equal("YGYYY", actualGame.Guesses[1].Score);

    Assert.Equal("CCCCC", actualGame.Guesses[2].Guess);
    Assert.Equal("YYGYY", actualGame.Guesses[2].Score);

    Assert.Equal("DDDDD", actualGame.Guesses[3].Guess);
    Assert.Equal("YYYGY", actualGame.Guesses[3].Score);

    Assert.Equal("EEEEE", actualGame.Guesses[4].Guess);
    Assert.Equal("YYYYG", actualGame.Guesses[4].Score);

    Assert.Equal("FFFFF", actualGame.Guesses[5].Guess);
    Assert.Equal("     ", actualGame.Guesses[5].Score);

  }

  [Fact]
  public async Task NotFoundIsReportedIfTheGameDoesNotExist()
  {
    var madeUpGameId = Guid.NewGuid();

    var response = await WhenTheGetRequestIsMade($"/game/{madeUpGameId}");
    await ThenNotFoundIsReturned(response, "The game does not exist.");
  }

  [Theory]
  [InlineData(GameState.InProgress, "")]
  [InlineData(GameState.Lost, "ABCDE")]
  [InlineData(GameState.Won, "ABCDE")]
  public async Task TheWordIsOnlyAvailableWhenTheGameIsComplete(GameState gameState, string expectedWord)
  {
    var game = await GivenAGame(word: "ABCDE",
                                with =>
                                {
                                  with.Guess1 = "AAAAA";
                                  with.State = gameState;
                                });

    var actualGame = await WhenTheGameIsRetrieved(game);
    Assert.NotNull(actualGame);
    Assert.Equal(expectedWord, actualGame.Word);
  }
}