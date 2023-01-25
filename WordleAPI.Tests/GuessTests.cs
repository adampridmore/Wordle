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
  public async Task GuessesMustBeFiveLetters()
  {
    var game = await GivenAGame("APPLE");

    var response = await WhenAGuessIsMade(game, "ABC");

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<string>();
    Assert.Equal("Guesses must be 5 letters long", detail);
  }

  [Fact]
  public async Task GuessesMustBeValidWords()
  {
    var game = await GivenAGame("APPLE");

    var response = await WhenAGuessIsMade(game, "ABCDE");

    Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    var detail = await response.Content.ReadFromJsonAsync<string>();
    Assert.Equal("Your guess is not a valid word", detail);
  }

  [Fact]
  public async Task CorrectLettersInCorrectPositionsAreMarkedGreen()
  {
    var game = await GivenAGame("ABCDE");

    var response = await WhenAGuessIsMade(game, "ABOUT");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("GG   ", detail!.Score);
  }

  [Fact]
  public async Task CorrectLettersInWrongPositionsAreMarkedYellow()
  {
    var game = await GivenAGame("EXXXA");

    var response = await WhenAGuessIsMade(game, "APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("Y   Y", detail!.Score);
    Assert.Equal(GameState.InProgress, detail.State);
  }

  [Fact]
  public async Task AGameCanBeWon()
  {
    var game = await GivenAGame("APPLE");

    var response = await WhenAGuessIsMade(game, "APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("GGGGG", detail!.Score);
    Assert.Equal(GameState.Won, detail.State);
  }

  [Fact]
  public async Task AGameCanBeLost()
  {
    var game = await GivenAGame("ABCDE",
                                wrongGuesses: new string[]{"AAAAA","AAAAA","AAAAA","AAAAA","AAAAA"});

    var response = await WhenAGuessIsMade(game, "APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("G   G", detail!.Score);
    Assert.Equal(GameState.Lost, detail.State);
  }

  private async Task<HttpResponseMessage> WhenAGuessIsMade(Game game, string guess)
  {
    var client = Client();
    var response = await client.PostAsJsonAsync("/guess", new NewGuess
    {
      GameId = game.Id,
      Guess = guess
    });
    return response;
  }

  private static async Task ThenOKIsReturned(HttpResponseMessage response)
  {
    if (response.StatusCode != HttpStatusCode.OK)
    {
      var detail = await response.Content.ReadAsStringAsync();
      Assert.Fail(detail);
    }
  }
}


// Duplicate Guess
// Guess when game has been won
// Guess when game has been lost
// Game does not exist