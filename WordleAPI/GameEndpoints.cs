using Microsoft.EntityFrameworkCore;

public static class GameEndpoints
{
  public static IEndpointRouteBuilder MapGameEndpoints(this IEndpointRouteBuilder endpoints)
  {
    endpoints.MapPost("/game", CreateNewGame);
    endpoints.MapGet("/game/{gameId}", GetGame);
    return endpoints;
  }

  async static Task<IResult> GetGame(WordleDb db, Guid gameId, Scorer scorer)
  {
    var game = await db.Games.Where(t => t.Id == gameId)
                                .FirstOrDefaultAsync();

    if (game is null)
    {
      return Results.NotFound("The game does not exist");
    }

    var result = new GetGameResponse
    {
      GameId = game.Id,
      Word = game.Word,
      State = game.State
    };

    var guesses = new List<GetGameResponse.GetGameScoreResponse>();
    AddGuess(scorer, game.Guess1, game.Word, guesses);
    AddGuess(scorer, game.Guess2, game.Word, guesses);
    AddGuess(scorer, game.Guess3, game.Word, guesses);
    AddGuess(scorer, game.Guess4, game.Word, guesses);
    AddGuess(scorer, game.Guess5, game.Word, guesses);
    AddGuess(scorer, game.Guess6, game.Word, guesses);
    result.Guesses = guesses.ToArray();

    return Results.Ok(result);

    static void AddGuess(Scorer scorer, string? guess, string actualWord, List<GetGameResponse.GetGameScoreResponse> guesses)
    {
      if (guess is not null)
      {
        guesses.Add(new GetGameResponse.GetGameScoreResponse
        {
          Guess = guess,
          Score = scorer.ScoreGuess(actualWord, guess)
        });
      }
    }
  }

  async static Task<IResult> CreateNewGame(NewGame gameDetails, WordleDb db, Words words)
  {
    var team = await db.Teams.Where(t => t.Id == gameDetails.TeamId)
                             .FirstOrDefaultAsync();
    if (team is null || gameDetails.TeamId == Guid.Empty)
    {
      return Results.NotFound("Team does not exist. Please call Team first.");
    }

    var game = new Game()
    {
      Id = Guid.NewGuid(),
      Team = team,
      State = GameState.InProgress,
      Word = words.RandomWord()
    };

    db.Games.Add(game);
    await db.SaveChangesAsync();

    var result = new NewGameResponse()
    {
      GameId = game.Id
    };

    return Results.Ok(result);
  }
}