using Microsoft.EntityFrameworkCore;

public static class GuessEndpoints
{
  public static IEndpointRouteBuilder MapGuessEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapPost("/guess", MakeGuess)
             .WithName("MakeGuess")
             .WithOpenApi(operation => new(operation)
             {
               Summary = "Make a guess at the word.",
               Description = "The game must be created first."
             })
             .Produces<NewGuessResponse>();

    return endpoints;
  }

  public static async Task<IResult> MakeGuess (NewGuess guessDetails, WordleDb db, Scorer scorer, Words words)
    {
      var game = await db.Games.Where(t => t.Id == guessDetails.GameId)
                             .FirstOrDefaultAsync();
      if (game is null || guessDetails.GameId == Guid.Empty)
      {
        return Results.NotFound("Game does not exist. Please call Game first.");
      }

      if (string.IsNullOrWhiteSpace(guessDetails.Guess))
      {
        return Results.BadRequest("A guess must be provided.");
      }

      var guess = guessDetails.Guess.ToUpper();
      if (guess.Length != 5)
      {
        return Results.BadRequest("Guesses must be 5 letters long");
      }

      if (game.State == GameState.Won)
      {
        return Results.BadRequest("You have already won this game.");
      }

      if (game.State == GameState.Lost)
      {
        return Results.BadRequest("You have already lost this game.");
      }

      if (!words.WordExists(guess))
        return Results.BadRequest("Your guess is not a valid word");

      if ((game.Guess1 == guess) ||
        (game.Guess2 == guess) ||
        (game.Guess3 == guess) ||
        (game.Guess4 == guess) ||
        (game.Guess5 == guess))
        return Results.BadRequest("You have already guessed this word.");

      if (game.Guess1 is null)
      {
        game.Guess1 = guess;
      }
      else if (game.Guess2 is null)
      {
        game.Guess2 = guess;
      }
      else if (game.Guess3 is null)
      {
        game.Guess3 = guess;
      }
      else if (game.Guess4 is null)
      {
        game.Guess4 = guess;
      }
      else if (game.Guess5 is null)
      {
        game.Guess5 = guess;
      }
      else if (game.Guess6 is null)
      {
        game.Guess6 = guess;
        game.State = GameState.Lost;
      }
      else
      {
        return Results.BadRequest("You have already had 6 guesses at getting this word.");
      }

      if (game.Word == guess)
      {
        game.State = GameState.Won;
      }

      await db.SaveChangesAsync();

      var score = scorer.ScoreGuess(game.Word, guess);

      return Results.Ok(new NewGuessResponse()
      {
        Score = score,
        State = game.State
      });
    }

}