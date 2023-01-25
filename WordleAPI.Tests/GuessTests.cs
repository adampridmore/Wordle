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
    var game = await GivenAGame(word:"APPLE");

    var response = await WhenAGuessIsMade(game, guess:"ABC");

    await ThenBadRequestIsReturned(response, "Guesses must be 5 letters long");
  }

  [Fact]
  public async Task GuessesMustBeValidWords()
  {
    var game = await GivenAGame(word:"APPLE");

    var response = await WhenAGuessIsMade(game, guess:"ABCDE");

    await ThenBadRequestIsReturned(response, "Your guess is not a valid word");
  }


  [Fact]
  public async Task CorrectLettersInCorrectPositionsAreMarkedGreen()
  {
    var game = await GivenAGame(word:"ABCDE");

    var response = await WhenAGuessIsMade(game, guess:"ABOUT");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("GG   ", detail!.Score);
    Assert.Equal(GameState.InProgress, detail.State);
  }

  [Fact]
  public async Task CorrectLettersInWrongPositionsAreMarkedYellow()
  {
    var game = await GivenAGame(word:"EXXXA");

    var response = await WhenAGuessIsMade(game, guess:"APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("Y   Y", detail!.Score);
    Assert.Equal(GameState.InProgress, detail.State);
  }

  [Fact]
  public async Task AGameCanBeWonWithTheCorrectGuess()
  {
    var game = await GivenAGame(word:"APPLE");

    var response = await WhenAGuessIsMade(game, guess:"APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("GGGGG", detail!.Score);
    Assert.Equal(GameState.Won, detail.State);
  }

  [Fact]
  public async Task AGameIsLostAfterSixWrongGuesses()
  {
    const string ANY_WRONG_GUESS = "VWXYZ";
    var game = await GivenAGame(word: "ABCDE",
                                with =>
                                {
                                  with.Guess1 = ANY_WRONG_GUESS;
                                  with.Guess2 = ANY_WRONG_GUESS;
                                  with.Guess3 = ANY_WRONG_GUESS;
                                  with.Guess4 = ANY_WRONG_GUESS;
                                  with.Guess5 = ANY_WRONG_GUESS;
                                });

    var response = await WhenAGuessIsMade(game, guess:"APPLE");

    await ThenOKIsReturned(response);

    var detail = await response.Content.ReadFromJsonAsync<NewGuessResponse>();
    Assert.Equal("G   G", detail!.Score);
    Assert.Equal(GameState.Lost, detail.State);
  }

  [Fact]
  public async Task AGuessCannotBeMadeForAGameWhichHasBeenWon()
  {
    var game = await GivenAGame(word: "APPLE",
                                with =>
                                {
                                  with.Guess1 = "APPLE";
                                  with.State = GameState.Won;
                                });

    var response = await WhenAGuessIsMade(game, guess:"APPLE");

    await ThenBadRequestIsReturned(response, "You have already won this game.");
  }

  [Fact]
  public async Task AGuessCannotBeMadeForAGameWhichHasBeenLost()
  {
    const string ANY_WRONG_GUESS = "VWXYZ";
    var game = await GivenAGame(word: "APPLE",
                                with =>
                                {
                                  with.Guess1 = ANY_WRONG_GUESS;
                                  with.Guess2 = ANY_WRONG_GUESS;
                                  with.Guess3 = ANY_WRONG_GUESS;
                                  with.Guess4 = ANY_WRONG_GUESS;
                                  with.Guess5 = ANY_WRONG_GUESS;
                                  with.Guess6 = ANY_WRONG_GUESS;
                                  with.State = GameState.Lost;
                                });

    var response = await WhenAGuessIsMade(game, ANY_WRONG_GUESS);

    await ThenBadRequestIsReturned(response, "You have already lost this game.");
  }

  [Fact]
  public async Task TheSameGuessCannotBeMadeTwice()
  {
    var game = await GivenAGame(word: "ABOUT",
                                with =>
                                {
                                  with.Guess1 = "APPLE";
                                });

    var response = await WhenAGuessIsMade(game, guess:"APPLE");

    await ThenBadRequestIsReturned(response, "You have already guessed this word.");
  }

  [Fact]
  public async Task TheGameMustExist()
  {
    var gameNotInDatabase = new Game();

    var response = await WhenAGuessIsMade(gameNotInDatabase, guess:"APPLE");

    await ThenNotFoundIsReturned(response, "Game does not exist. Please call Game first.");
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
}

// Guesses are written to the database